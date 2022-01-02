using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.Runtime.CompilerServices;

namespace SpectrumInformationDistribution
{
    public class ProcessMusic
    {
        private WasapiCapture _soundIn;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        public float ampMult = 4f;
        public float ampMultiplier;
        private int currentRollingWindowIndex;
        public float[] fftBuffer;
        public const FftSize fftSize = FftSize.Fft1024;
        public float firstHeardSound;
        public float lastHeardSound;
        private float[] lastSamples;
        public bool lastUseLoopback;
        public bool lastUseMic;
        public bool newSong = true;
        private float nextRollingWindowFrameAt;
        public float noMusicSilenceTime;
        public int numSamples = 64;
        public float rollingMaxSample;
        private float[] rollingMaxWindow;
        private int rollingWindow;
        public float rollingWindowFPS = 90f;
        private float rollingWindowFrameTime;
        public float rollingWindowTime = 10f;
        public float sampleMult = 3f;
        public float[] samples;
        private BasicSpectrumProvider spectrumProvider;
        public bool useLoopback = true;
        public bool useMic;
        float timeWithRun;

        private float TimeFromStartup {
            get {
                return TimerManager.CurrentTime - timeWithRun;
            }
        }

        public float AudioAmp
        {
            get
            {
                return this.ampMultiplier;
            }
        }

        public float AudioAmpSum
        {
            get
            {
                float sum = 0;
                for (int i = 0; i < samples.Length; i++)
                {
                    sum += samples[i];
                }
                return sum;
            }
        }

        public void Init()
        {
            this.samples = new float[this.numSamples];
            this.fftBuffer = new float[1024];

            this.lastHeardSound = -1f;
            this.firstHeardSound = -1f;
            this.rollingWindow = (int)(this.rollingWindowTime * this.rollingWindowFPS);
            this.rollingMaxWindow = new float[this.rollingWindow];
            this.lastSamples = new float[this.numSamples];
            this.rollingWindowFrameTime = 1f / this.rollingWindowFPS;
            this.currentRollingWindowIndex = 0;
            this.timeWithRun = TimerManager.CurrentTime;

            this.useMic = false;
            this.useLoopback = true;
            this.lastUseLoopback = false;
            this.lastUseMic = false;
        }

        public void Stop()
        {
            if (this._soundOut != null)
            {
                this._soundOut.Stop();
                this._soundOut.Dispose();
                this._soundOut = null;
            }
            if (this._soundIn != null)
            {
                this._soundIn.Stop();
                this._soundIn.Dispose();
                this._soundIn = null;
            }
            if (this._source != null)
            {
                this._source.Dispose();
                this._source = null;
            }
        }

        public void OnUpdate()
        {
            if (this.nextRollingWindowFrameAt <= TimerManager.CurrentTime)
            {
                if ((this.useMic != this.lastUseMic) || (this.useLoopback != this.lastUseLoopback))
                {
                    this.enableMic(this.useMic);
                    this.enableLoopback(this.useLoopback);
                }
                this.rollingMaxSample = 0f;
                if ((this.spectrumProvider != null) && this.spectrumProvider.GetFftData(this.fftBuffer, this))
                {
                    for (int m = 0; m < this.samples.Length; m++)
                    {
                        this.samples[m] = this.fftBuffer[m + 2] + ((2f * (float)Math.Sqrt((float)(m / this.samples.Length))) * this.fftBuffer[m + 2]);
                        this.lastSamples[m] = this.samples[m];
                        if (this.samples[m] > this.rollingMaxSample)
                        {
                            this.rollingMaxSample = this.samples[m];
                        }
                    }
                }
                else
                {
                    for (int n = 0; n < this.samples.Length; n++)
                    {
                        this.samples[n] = this.lastSamples[n];
                        if (this.samples[n] > this.rollingMaxSample)
                        {
                            this.rollingMaxSample = this.samples[n];
                        }
                    }
                }
                if (this.rollingMaxSample >= 0.001f)
                {
                    this.lastHeardSound = TimerManager.CurrentTime;
                }
                if ((this.rollingMaxSample >= 0.001f) && (this.firstHeardSound < 0.001f))
                {
                    this.firstHeardSound = TimerManager.CurrentTime;
                }
                this.rollingMaxWindow[this.currentRollingWindowIndex % this.rollingMaxWindow.Length] = this.rollingMaxSample;
                for (int i = 0; i < MathExtension.Clamp(this.currentRollingWindowIndex, 0, this.rollingWindow); i++)
                {
                    if (this.rollingMaxSample < this.rollingMaxWindow[i])
                    {
                        this.rollingMaxSample = this.rollingMaxWindow[i];
                    }
                }
                this.ampMultiplier = 0f;
                for (int j = 0; j < this.numSamples; j++)
                {
                    this.samples[j] /= this.rollingMaxSample + 1E-06f;
                    this.samples[j] *= this.sampleMult;
                    this.ampMultiplier += this.samples[j];
                }
                this.ampMultiplier /= (float)this.numSamples;
                this.ampMultiplier *= this.ampMult;
                for (int k = 0; k < MathExtension.Clamp(this.currentRollingWindowIndex, 0, this.rollingWindow); k++)
                {
                    this.rollingMaxWindow[k] *= 0.995f;
                }
                this.currentRollingWindowIndex++;
                this.nextRollingWindowFrameAt = TimeFromStartup + this.rollingWindowFrameTime;

                //Debug.Log(this.ampMultiplier);

            }
        }

        private void enableLoopback(bool loopbackEnabled)
        {
            if (loopbackEnabled && (this.lastUseLoopback != loopbackEnabled))
            {
                this.lastUseLoopback = loopbackEnabled;
                this.InitializeCapture("loopback");
            }
            else if (!loopbackEnabled && (this.lastUseLoopback != loopbackEnabled))
            {
                this.lastUseLoopback = loopbackEnabled;
            }
        }

        private void enableMic(bool micEnabled)
        {
            this.Stop();
            if (micEnabled && (this.lastUseMic != micEnabled))
            {
                this.lastUseMic = micEnabled;
                this.InitializeCapture("mic");
            }
            else if (!micEnabled && (this.lastUseMic != micEnabled))
            {
                this.lastUseMic = micEnabled;
            }
        }

        private void InitializeCapture(string device)
        {
            //设置设备名
            this.Stop();
            if (device == "loopback")
            {
                this._soundIn = new WasapiLoopbackCapture(20);
                this._soundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
            else
            {
                this._soundIn = new WasapiCapture(true, AudioClientShareMode.Shared, 20);
                this._soundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
            }
            this._soundIn.Initialize();
            SoundInSource waveSource = new SoundInSource(this._soundIn);
            ISampleSource aSampleSource = waveSource.ToSampleSource();  //采样音频
            this.SetupSampleSource(aSampleSource);
            waveSource.DataAvailable += new EventHandler<DataAvailableEventArgs>(OnDataAvaliable);
            this._soundIn.Start();
        }

        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            this.spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels, aSampleSource.WaveFormat.SampleRate, FftSize.Fft1024);
            SingleBlockNotificationStream sampleSource = new SingleBlockNotificationStream(aSampleSource);
            sampleSource.SingleBlockRead += (s, a) => { this.spectrumProvider.Add(a.Left, a.Right); };
            this._source = sampleSource.ToWaveSource(16);
        }

        private void OnDataAvaliable(object s, DataAvailableEventArgs e)
        {
            this._source.Read(e.Data, e.Offset, e.ByteCount);
        }

    }
}




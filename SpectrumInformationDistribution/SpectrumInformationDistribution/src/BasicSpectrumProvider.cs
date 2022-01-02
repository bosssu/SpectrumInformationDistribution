using CSCore.DSP;
using System;
using System.Collections.Generic;
namespace SpectrumInformationDistribution
{
    public class BasicSpectrumProvider : FftProvider, ISpectrumProvider
    {
        private readonly List<object> _contexts;
        private readonly int _sampleRate;

        public BasicSpectrumProvider(int channels, int sampleRate, FftSize fftSize) : base(channels, fftSize)
        {
            this._contexts = new List<object>();
            if (sampleRate <= 0)
            {
                throw new ArgumentOutOfRangeException("sampleRate");
            }
            this._sampleRate = sampleRate;
        }

        public override void Add(float[] samples, int count)
        {
            base.Add(samples, count);
            if (count > 0)
            {
                this._contexts.Clear();
            }
        }

        public override void Add(float left, float right)
        {
            base.Add(left, right);
            this._contexts.Clear();
        }

        public int GetFftBandIndex(float frequency)
        {
            int fftSize = (int)base.FftSize;
            double num2 = ((double)this._sampleRate) / 2.0;
            return (int)((((double)frequency) / num2) * (fftSize / 2));
        }

        public bool GetFftData(float[] fftResultBuffer, object context)
        {
            if (this._contexts.Contains(context))
            {
                return false;
            }
            this._contexts.Add(context);
            this.GetFftData(fftResultBuffer);
            return true;
        }
    }
}



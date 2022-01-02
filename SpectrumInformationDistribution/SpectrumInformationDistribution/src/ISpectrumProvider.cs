

namespace SpectrumInformationDistribution
{
    public interface ISpectrumProvider
    {
        int GetFftBandIndex(float frequency);
        bool GetFftData(float[] fftBuffer, object context);
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumInformationDistribution
{
    class MathExtension
    {
        public static float Clamp(float srcValue, float min, float max)
        {
            float final = srcValue;
            if (final <= min)
            {
                final = min;
            }
            else if(final > max)
            {
                final = max;
            }
            return final;
        }
    }
}

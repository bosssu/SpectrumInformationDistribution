using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumInformationDistribution
{
    class TimerManager
    {

        public static float CurrentTime
        {
            get
            {
                TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
                return (float)ts.TotalSeconds;
            }
        }

    }
}

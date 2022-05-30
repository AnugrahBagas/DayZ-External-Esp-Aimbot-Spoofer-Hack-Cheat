using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11
{
    public class FpsRenderer
    {
        Stopwatch clock;
        double totalTime;
        long frameCount;
        double measuredFPS;

        public FpsRenderer()
        {
        }

        public bool Show { get; set; }

        public virtual void Initialize()
        {
            clock = Stopwatch.StartNew();
        }

        public double Calc()
        {
            frameCount++;
            var timeElapsed = (double)clock.ElapsedTicks / Stopwatch.Frequency; ;
            totalTime += timeElapsed;
            if (totalTime >= 1.0f)
            {
                measuredFPS = (double)frameCount / totalTime;
                frameCount = 0;
                totalTime = 0.0;
            }

            clock.Restart();

            return measuredFPS;
        }
    }
}

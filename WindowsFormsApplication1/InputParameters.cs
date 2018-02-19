using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellFish
{
    class InputParameters
    {

        public int MinHoldTime;
        public int MaxHoldTime;
        public int SleepTime;

        public InputParameters(int MinHoldTime, int MaxHoldTime, int SleepTime)
        {
            this.MinHoldTime = MinHoldTime;
            this.MaxHoldTime = MaxHoldTime;
            this.SleepTime = SleepTime;

        }
    }
}

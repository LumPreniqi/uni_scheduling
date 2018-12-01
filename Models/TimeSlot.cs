using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class TimeSlot
    {
        public int Day { get; }
        public int Period { get; }

        public TimeSlot(int Day, int Period)
        {
            this.Day = Day;
            this.Period = Period;
        }
    }
}

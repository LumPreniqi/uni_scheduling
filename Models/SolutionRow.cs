using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class SolutionRow
    {
        public string CourseId { get; set; }
        public string RoomId { get; set; }
        public int StartSlot { get; set; }
        public int EndSlot { get; set; }
        public int Period { get; set; }

        public SolutionRow(string CourseId, string RoomId, int Day, int Period)
        {
            this.CourseId = CourseId;
            this.RoomId = RoomId;
            this.Day = Day;
            this.Period = Period;
        }
    }
}

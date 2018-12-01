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
        public string TeacherId { get; set; }
        public int Day { get; set; }

        public SolutionRow(Course Course, string RoomId, int StartSlot, int EndSlot, int Day)
        {
            this.CourseId = Course.Id;
            this.RoomId = RoomId;
            this.StartSlot = StartSlot;
            this.EndSlot = EndSlot;
            this.Day = Day;
            this.TeacherId = Course.TeacherId;
        }
    }
}

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
        public bool IsRoomCorrect { get; set; }
        public Course Course { get; set; }

        public SolutionRow(Course Course, Room Room, int StartSlot, int EndSlot, int Day)
        {
            this.CourseId = Course.Id;
            this.Course = Course;
            this.RoomId = Room.Id;
            this.StartSlot = StartSlot;
            this.EndSlot = EndSlot;
            this.Day = Day;
            this.TeacherId = Course.TeacherId;
            this.IsRoomCorrect = Course.Students <= Room.Size;
        }

        public override string ToString()
        {
            return this.CourseId + " " + this.RoomId + " " + this.Day + " " + this.StartSlot;
        }
    }
}

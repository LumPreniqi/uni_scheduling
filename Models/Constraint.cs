using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class Constraint
    {
        public string Type { get; }
        public string CourseId { get; }
        public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
        public List<Room> Rooms { get; set; } = new List<Room>();

        public Constraint(string Type, string CourseId)
        {
            this.Type = Type;
            this.CourseId = CourseId;
        }
    }
}

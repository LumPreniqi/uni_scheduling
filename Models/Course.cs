using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class Course
    {
        public string Id { get; }
        public string TeacherId { get; }
        public int Lectures { get; }
        public int Students { get; }

        public Course(string Id, string TeacherId, int Lectures, int Students)
        {
            this.Id = Id;
            this.TeacherId = TeacherId;
            this.Lectures = Lectures;
            this.Students = Students;
        }

        public int GetSlots()
        {
            int periods = 0;
            int timeSlots = this.Lectures * 3;

            if(this.Lectures <= 3)
            {
                periods = 0;
            }
            else if (this.Lectures <= 6)
            {
                periods = 1;
            }
            else if(this.Lectures <= 8)
            {
                periods = 2;
            }

            return timeSlots + periods;
        }
    }
}

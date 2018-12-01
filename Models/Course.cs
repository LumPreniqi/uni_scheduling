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

        public int GetCourseSlots()
        {
            // TO DO llogariti slots e kursit (lecture * 3 + pushimet varet qysh i bjen)  
            return Lectures;
        }
    }
}

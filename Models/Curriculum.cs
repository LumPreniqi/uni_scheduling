using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class Curriculum
    {
        public string Id { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

        public Curriculum(string Id)
        {
            this.Id = Id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class Room
    {
        public string Id { get; }
        public int Size { get; }
        public Room(string Id, int Size)
        {
            this.Id = Id;
            this.Size = Size;
        }
    }
}

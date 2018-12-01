using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using UniScheduling.Models;

namespace UniScheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            IO.Read();
            Solution.Generate();
            Console.ReadKey();
        }
    }
}

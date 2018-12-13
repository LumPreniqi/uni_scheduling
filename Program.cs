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
            var solutions = Solution.Generate();

            var fitness = Fitness.CheckFitness(solutions, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula);
            Console.WriteLine("Fitness: {0}", fitness);

            solutions = Solution.ChangeCourseRoom(solutions);

            fitness = Fitness.CheckFitness(solutions, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula);
            Console.WriteLine("Fitness: {0}", fitness);

            IO.Write(solutions);

            Console.WriteLine("DONE!");
            Console.ReadKey();
        }
    }
}

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

            Console.WriteLine("Initial Fitness: {0}", Fitness.CheckFitness(solutions, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula));

            var best = Solution.FindBetterSolution(solutions, 10);

            Console.WriteLine("Initial Fitness: {0}", Fitness.CheckFitness(solutions, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula));
            Console.WriteLine("Best Fitness: {0}", Fitness.CheckFitness(best, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula));

            // IO.Write(solutions);

            Console.WriteLine("DONE!");
            Console.ReadKey();
        }

    }
}

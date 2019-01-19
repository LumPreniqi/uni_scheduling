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
            var input = "FME-S";
            IO.Read(input);
            var solutions = Solution.Generate();

            Console.WriteLine("Initial Fitness: {0}", Fitness.CheckFitness(solutions, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula));

            var best = Solution.FindBetterSolution(solutions, 3);

            Console.WriteLine("Initial Fitness: {0}", Fitness.CheckFitness(solutions, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula));
            var score = Fitness.CheckFitness(best, Solution.Days, Solution.Courses, Solution.Rooms, Solution.Curricula);
            Console.WriteLine("Best Fitness: {0}", score);

            IO.Write(best, input + " Solution - " + score);

            Console.WriteLine("DONE!");
            Console.ReadKey();
        }

    }
}

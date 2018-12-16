using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class Fitness
    {
        public static int CheckFitness(List<SolutionRow> solutions, int Days, List<Course> Courses,
            List<Room> Rooms, List<Curriculum> Curricula)
        {
            var roomCapacity = CheckRoomSuitability(solutions, Courses, Rooms);
            var windows = CheckWindows(solutions, Days, Courses, Curricula);

            return roomCapacity + windows;
        }

        public static int CheckRoomSuitability(List<SolutionRow> solutions, List<Course> Courses, List<Room> Rooms)
        {
            var roomCapacity = 0;

            foreach (var solution in solutions)
            {
                var course = Courses.FirstOrDefault(x => x.Id == solution.CourseId);
                var room = Rooms.FirstOrDefault(x => x.Id == solution.RoomId);

                if (course != null && room != null && course.Students > room.Size)
                {
                    roomCapacity++;
                }
            }

            return roomCapacity;
        }

        public static int CheckWindows(List<SolutionRow> solutions, int Days, List<Course> Courses, List<Curriculum> Curricula)
        {
            var windows = 0;

            foreach (var curricula in Curricula)
            {
                for (int day = 0; day < Days; day++)
                {
                    var sameCurricula = new List<SolutionRow>();

                    foreach (var s in solutions.Where(x => x.Day == day))
                    {
                        if (curricula.Courses.Contains(Courses.First(x => x.Id == s.CourseId)))
                        {
                            sameCurricula.Add(s);
                        }
                    }

                    sameCurricula = sameCurricula.OrderBy(x => x.StartSlot).ToList();

                    for (int i = 1; i < sameCurricula.Count; i++)
                    {
                        var difference = sameCurricula[i].StartSlot - sameCurricula[i - 1].EndSlot;
                        if (difference > 1)
                        {
                            windows += difference;
                        }
                    }
                }
            }

            return windows;
        }

        public static List<SolutionRow> GetWindows(List<SolutionRow> solutions, int day, List<Course> Courses, List<Curriculum> Curricula)
        {
            foreach (var curricula in Curricula)
            {
                var sameCurricula = new List<SolutionRow>();

                foreach (var s in solutions.Where(x => x.Day == day))
                {
                    if (curricula.Courses.Contains(Courses.First(x => x.Id == s.CourseId)))
                    {
                        sameCurricula.Add(s);
                    }
                }

                sameCurricula = sameCurricula.OrderBy(x => x.StartSlot).ToList();

                for (int i = 1; i < sameCurricula.Count; i++)
                {
                    var difference = sameCurricula[i].StartSlot - sameCurricula[i - 1].EndSlot;
                    if (difference > 1)
                    {
                        return sameCurricula;
                    }
                }   
            }

            return null;
        }
    }
}

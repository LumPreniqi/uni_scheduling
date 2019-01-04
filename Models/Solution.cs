using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScheduling.Models
{
    class Solution
    {
        public static int Days = 5;
        public static int SlotsPerDay = 48;
        private static string RoomId;

        public static List<Course> Courses { get; set; } = new List<Course>();
        public static List<Constraint> Constraints { get; set; } = new List<Constraint>();
        public static List<Room> Rooms { get; set; } = new List<Room>();
        public static List<Curriculum> Curricula { get; set; } = new List<Curriculum>();


        public static List<SolutionRow> Generate()
        {
            var solutions = new List<SolutionRow>();

            foreach (var course in Courses)
            {
                var availableRooms = GetRooms(course);
                var curriculaCourses = GetCurriculumCourses(course);
                var lectureSlots = course.GetSlots();

                for (int day = 0; day < Days; day++)
                {
                    var startSlot = FindSolution(solutions, course, availableRooms, curriculaCourses, day, lectureSlots);
                    if (startSlot != -1)
                    {
                        solutions.Add(new SolutionRow(course, RoomId, startSlot, startSlot + lectureSlots, day));
                        Console.WriteLine("{0} {1} {2} {3}", course.Id, RoomId, day, startSlot);

                        //Console.WriteLine("Course ID: {0} / Teacher ID: {1} / Room ID: {2} / Day: {3} / Slot: {4}", course.Id, course.TeacherId, RoomId, day, startSlot);
                        break;
                    }
                }
            }

            return solutions;
        }

        public static List<Room> GetRooms(Course course)
        {
            var suitableRooms = Rooms.ToList(); // Rooms.Where(room => course.Students <= room.Size).ToList();
            var roomConstraint = Constraints.Where(constraint => constraint.Type == "room" && constraint.CourseId == course.Id).SelectMany(x => x.Rooms).ToList();
            suitableRooms = suitableRooms.Except(roomConstraint).ToList();

            return suitableRooms;
        }

        public static List<TimeSlot> GetNotAllowedSlots(Course course, int Day)
        {
            var periodConstraint = Constraints.Where(constraint => constraint.Type == "period" && constraint.CourseId == course.Id).SelectMany(x => x.TimeSlots).ToList();
            periodConstraint = periodConstraint.Where(x => x.Day == Day).ToList();

            return periodConstraint;
        }

        public static List<Course> GetCurriculumCourses(Course course)
        {
            var curriculms = Curricula.Where(x => x.Courses.Contains(course)).ToList();
            var curriculaCourses = curriculms.SelectMany(x => x.Courses).Distinct().ToList();
            curriculaCourses.Remove(course);

            return curriculaCourses;
        }

        public static int FindSolution(List<SolutionRow> solutions, Course course, List<Room> availableRooms, List<Course> curriculumCourses, int day, int lectureSlots)
        {
            var notAllowedSlots = GetNotAllowedSlots(course, day);

            for (int slot = 0; slot < SlotsPerDay - lectureSlots; slot++)
            {
                // check if we have, not allowed slots
                var matchSlot = notAllowedSlots.FirstOrDefault(x => x.Period >= slot && x.Period <= (slot + lectureSlots));
                if (matchSlot != null)
                {
                    continue;
                }

                // get solutions in the same slot or interval
                var getExistingSolutions = solutions.Where(x => x.Day == day).Where(x => (x.StartSlot <= slot && slot <= x.EndSlot) ||
                (x.StartSlot <= (slot + lectureSlots) && (slot + lectureSlots) <= x.EndSlot) ||
                (slot <= x.StartSlot && (slot + lectureSlots) >= x.EndSlot)).ToList();

                // check if we have the same teacher
                var sameTeacher = getExistingSolutions.FirstOrDefault(x => x.TeacherId == course.TeacherId);
                if (sameTeacher != null)
                {
                    if (sameTeacher.EndSlot > slot)
                    {
                        slot = sameTeacher.EndSlot;
                    }
                    continue;
                }

                // check if have the same curricula
                var sameCurricula = CheckCurricula(getExistingSolutions, curriculumCourses);
                if (sameCurricula != null)
                {
                    if (sameCurricula.EndSlot > slot)
                    {
                        slot = sameCurricula.EndSlot;
                    }
                    continue;
                }

                var unAvailableRoomSolutions = new List<SolutionRow>();
                var filteredAvailableRooms = new List<Room>();

                // Remove unavailable rooms
                foreach (var availableRoom in availableRooms)
                {
                    var sameRoom = getExistingSolutions.FirstOrDefault(x => x.RoomId == availableRoom.Id);

                    filteredAvailableRooms.Add(availableRoom);
                    unAvailableRoomSolutions.Add(sameRoom);

                    if (sameRoom != null)
                        filteredAvailableRooms.Remove(availableRoom);
                }

                if (filteredAvailableRooms.Any())
                {
                    var rnd = new Random();
                    RoomId = filteredAvailableRooms[rnd.Next(filteredAvailableRooms.Count)].Id;
                    // RoomId = GetMostSuitableRoom(filteredAvailableRooms, course.Students);
                }
                else
                {
                    int lowestEndSlot = unAvailableRoomSolutions.OrderBy(slt => slt.EndSlot).First().EndSlot;

                    if (lowestEndSlot > slot)
                    {
                        slot = lowestEndSlot;
                    }
                    continue;
                }

                return slot;
            }

            return -1;
        }

        public static SolutionRow CheckCurricula(List<SolutionRow> existingSolutions, List<Course> curriculumCourses)
        {
            foreach (var solution in existingSolutions)
            {
                var sameCurricula = curriculumCourses.FirstOrDefault(x => x.Id == solution.CourseId);
                if (sameCurricula != null)
                {
                    return solution;
                }
            }

            return null;
        }

        public static string GetMostSuitableRoom(List<Room> rooms, int students)
        {
            return rooms.OrderBy(rm => Math.Abs(rm.Size - students)).First().Id;
        }

        public static List<SolutionRow> ChangeCourseRoom(List<SolutionRow> s)
        {
            var solutions = new List<SolutionRow>(s);

            var rnd = new Random();
            var index = rnd.Next(Courses.Count);
            var course = Courses[index];
            var availableRooms = GetRooms(course);
            var solutionIndex = solutions.FindIndex(x => x.CourseId == course.Id);
            if (solutionIndex == -1)
            {
                return solutions;
            }
            availableRooms.Remove(Rooms.First(x => x.Id == solutions[solutionIndex].RoomId));

            var getExistingSolutions = solutions.Where(x => x.Day == solutions[solutionIndex].Day)
                .Where(x => (x.StartSlot <= solutions[solutionIndex].StartSlot && solutions[solutionIndex].StartSlot <= x.EndSlot) ||
                (x.StartSlot <= solutions[solutionIndex].EndSlot && solutions[solutionIndex].EndSlot <= x.EndSlot) ||
                (solutions[solutionIndex].StartSlot <= x.StartSlot && solutions[solutionIndex].EndSlot >= x.EndSlot)).ToList();

            var busyRoomIds = getExistingSolutions.Select(x => x.RoomId).ToList();
            availableRooms = availableRooms.Where(x => !busyRoomIds.Contains(x.Id)).ToList();

            if (course.Students < Rooms.First(x => x.Id == solutions[solutionIndex].RoomId).Size)
            {
                return solutions;
            }
            
            if (availableRooms.Count() == 0)
            {
                return solutions;
            }

            var bestAvailableRooms = availableRooms.Where(x => course.Students < x.Size).ToList();

            if (bestAvailableRooms.Count() == 0)
            {
                var solutionRow = solutions[solutionIndex];
                solutions.Remove(solutionRow);
                solutions.Add(new SolutionRow(course, availableRooms[rnd.Next(availableRooms.Count)].Id, solutionRow.StartSlot, solutionRow.EndSlot, solutionRow.Day));
            }
            else
            {
                bestAvailableRooms = bestAvailableRooms.OrderBy(x => x.Size).ToList();
                var solutionRow = solutions[solutionIndex];
                solutions.Remove(solutionRow);
                solutions.Add(new SolutionRow(course, bestAvailableRooms.First().Id, solutionRow.StartSlot, solutionRow.EndSlot, solutionRow.Day));
            }

            return solutions;
        }

        public static List<SolutionRow> SwapCourses(List<SolutionRow> s)
        {
            var solutions = new List<SolutionRow>(s);
            var rnd = new Random();
            var dayIndex = rnd.Next(Days);
            var daySolutions = solutions.Where(x => x.Day == dayIndex);

            var sameCurricula = Fitness.GetWindows(solutions, dayIndex, Courses, Curricula);
            if (sameCurricula == null)
            {
                return solutions;
            }

            var notCorrectSolution = new List<SolutionRow>();
            var hasDifference = false;

            for (int i = 1; i < sameCurricula.Count; i++)
            {
                var difference = sameCurricula[i].StartSlot - sameCurricula[i - 1].EndSlot;
                if (difference > 1 || hasDifference)
                {
                    hasDifference = true;
                    var course = daySolutions.First(x => x.CourseId == sameCurricula[i].CourseId);
                    solutions.Remove(course);
                    solutions = FindSlot(dayIndex, Courses.First(x => x.Id == course.CourseId), solutions);
                }
            }

            return solutions;
        }

        public static List<SolutionRow> FindSlot(int existingDay, Course course, List<SolutionRow> solutions)
        {
            var availableRooms = GetRooms(course);
            var curriculaCourses = GetCurriculumCourses(course);
            var lectureSlots = course.GetSlots();

            for (int day = 0; day < Days; day++)
            {
                if (day == existingDay)
                {
                    continue;
                }

                var startSlot = FindSolution(solutions, course, availableRooms, curriculaCourses, day, lectureSlots);
                if (startSlot != -1)
                {
                    solutions.Add(new SolutionRow(course, RoomId, startSlot, startSlot + lectureSlots, day));
                    //Console.WriteLine("{0} {1} {2} {3}", course.Id, RoomId, day, startSlot);
                    break;
                }
            }

            return solutions;
        }

        public static List<SolutionRow> FindBetterSolution(List<SolutionRow> solutions, int iterations)
        {
            var best = new List<SolutionRow>(solutions);
            var s = new List<SolutionRow>(solutions);
            var h = new List<SolutionRow>(solutions);

            var fitness1 = 0;
            var fitness2 = 0;

            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < iterations; j++)
                {
                    var r = ChangeCourseRoom(s);

                    fitness1 = Fitness.CheckFitness(r, Days, Courses, Rooms, Curricula);
                    fitness2 = Fitness.CheckFitness(s, Days, Courses, Rooms, Curricula);
                    s = fitness1 > fitness2 ? s : r;
                }

                fitness1 = Fitness.CheckFitness(s, Days, Courses, Rooms, Curricula);
                fitness2 = Fitness.CheckFitness(best, Days, Courses, Rooms, Curricula);
                Console.WriteLine("Fitness S: " + fitness1.ToString());
                Console.WriteLine("Fitness Best: " + fitness2.ToString());
                best = fitness1 > fitness2 ? best : s;

                h = NewHomeBase(h, s);
                s = Perturb(h);
            }

            return best;
        }

        public static List<SolutionRow> Perturb(List<SolutionRow> s)
        {
            var solutions = new List<SolutionRow>(s);

            for (int i = 0; i < 5; i++)
            {
                solutions = SwapCourses(solutions);
            }

            return solutions;
        }

        public static List<SolutionRow> NewHomeBase(List<SolutionRow> h, List<SolutionRow> s)
        {
            var fitness1 = Fitness.CheckFitness(h, Days, Courses, Rooms, Curricula);
            var fitness2 = Fitness.CheckFitness(s, Days, Courses, Rooms, Curricula);

            return fitness1 >= fitness2 ? s : h;
        }

    }
}
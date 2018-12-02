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

        public static bool found = false;

        public static void Generate()
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
                        Console.WriteLine("Course ID: {0} Room ID: {1} Day: {2} Slot: {3}", course.Id, RoomId, day, startSlot);
                        solutions.Add(new SolutionRow(course, RoomId, startSlot, startSlot + lectureSlots, day));
                        break;
                    }
                }
            }

            IO.Write(solutions);
        }

        public static List<Room> GetRooms(Course course)
        {
            var suitableRooms = Rooms.Where(room => course.Students <= room.Size).ToList();
            var roomConstraint = Constraints.Where(constraint => constraint.Type == "room" && constraint.CourseId == course.Id).SelectMany(x => x.Rooms).ToList();
            suitableRooms = suitableRooms.Except(roomConstraint).ToList();
            var rnd = new Random();

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
                (x.StartSlot <= (slot + lectureSlots) && (slot + lectureSlots) <= x.EndSlot)).ToList();

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

                // Remove unavailable rooms
                foreach (var availableRoom in availableRooms)
                {
                    var sameRoom = getExistingSolutions.FirstOrDefault(x => x.RoomId == availableRoom.Id);
                    availableRooms.Except(Rooms.Where(rm => rm.Id == sameRoom.RoomId));
                }
                    
                if(availableRooms != null)
                {
                    Random rand = new Random();
                    RoomId = availableRooms[rand.Next(availableRooms.Count)].Id;
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScheduling.Enums;

namespace UniScheduling.Models
{
    class Solution
    {
        public static int Days = 5;
        public static int CurrentDay;

        public static List<Course> Courses { get; set; } = new List<Course>();
        public static List<Constraint> Constraints { get; set; } = new List<Constraint>();
        public static List<Room> Rooms { get; set; } = new List<Room>();
        public static List<Curriculum> Curricula { get; set; } = new List<Curriculum>();

        public static void Generate()
        {
            var solutions = new List<SolutionRow>();
            bool found;
            foreach (Course course in Courses)
            {
                found = false;
                while(!found)
                {
                    for (CurrentDay = 0; CurrentDay < Days; CurrentDay++)
                    {

                    }
                }

                solutions.Add(new SolutionRow(course.Id, "", 0, 0));
            }

        }

        public static string GetRoom(Course course)
        {
            var suitableRooms = Rooms.Where(room => course.Students <= room.Size).ToList();
            var roomConstraint = Constraints.Where(constraint => constraint.Type == "room" && constraint.CourseId == course.Id);

            if(roomConstraint.Count() > 0)
            {
                foreach (var invalidRoom in roomConstraint.First().Rooms)
                {
                    if(suitableRooms.Contains(invalidRoom))
                    {
                        suitableRooms.Remove(invalidRoom);
                    }
                }
            }

            return suitableRooms.First().Id;
        }
        public static int GetPeriod(Course course, int Day)
        {
            int EndTimeSlot = 0;

            var periodConstraint = Constraints.Where(constraint => constraint.Type == "period" && constraint.CourseId == course.Id);
            if (periodConstraint.Count() > 0)
            {
                EndTimeSlot = (periodConstraint.First().TimeSlots.Where(t => t.Day == Day).Last().Period  + 1;
            }

            return EndTimeSlot;
        }

        public static bool CheckCurriculum(Course course)
        {
            return false;
        }

        public bool CheckCurrentSoluton(SolutionRow solution)
        {
            return false;
        }
        public static bool CheckConstraints(Course course)
        {
            return false;
        }

    }
}
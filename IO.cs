using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScheduling.Models;

namespace UniScheduling
{
    class IO
    {
        public static void Read()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\TechStore\\source\\repos\\UniScheduling\\UniScheduling\\Input\\input.xml");
            XmlElement root = doc.DocumentElement;

            XmlNode parentNode = root.SelectSingleNode("courses");
            foreach (XmlNode course in parentNode.ChildNodes)
            {
                Solution.Courses.Add(new Course(course.Attributes["id"].Value, course.Attributes["teacher"].Value,
                                                Convert.ToInt32(course.Attributes["lectures"].Value),
                                                Convert.ToInt32(course.Attributes["students"].Value)));
            }

            parentNode = root.SelectSingleNode("rooms");
            foreach (XmlNode room in parentNode.ChildNodes)
            {
                Solution.Rooms.Add(new Room(room.Attributes["id"].Value, Convert.ToInt32(room.Attributes["size"].Value)));
            }

            parentNode = root.SelectSingleNode("constraints");
            foreach (XmlNode constraint in parentNode.ChildNodes)
            {
                Constraint baseConstraint = new Constraint(constraint.Attributes["type"].Value, constraint.Attributes["course"].Value);
                foreach (XmlNode childConstraint in constraint.ChildNodes)
                {
                    if (constraint.Attributes["type"].Value == "period")
                    {
                        baseConstraint.TimeSlots.Add(new TimeSlot(
                            Convert.ToInt32(childConstraint.Attributes["day"].Value),
                            Convert.ToInt32(childConstraint.Attributes["period"].Value)));
                    }
                    else
                    {
                        Room existingRoom = Solution.Rooms.Find(rm => rm.Id == childConstraint.Attributes["ref"].Value);
                        baseConstraint.Rooms.Add(existingRoom);
                    }
                }

                Solution.Constraints.Add(baseConstraint);
            }

            parentNode = root.SelectSingleNode("curricula");
            foreach (XmlNode curriculum in parentNode.ChildNodes)
            {
                Curriculum baseCurriculum = new Curriculum(curriculum.Attributes["id"].Value);
                foreach (XmlNode course in curriculum.ChildNodes)
                {
                    Course existingCourse = Solution.Courses.Find(crs => crs.Id == course.Attributes["ref"].Value);
                    baseCurriculum.Courses.Add(existingCourse);
                }

                Solution.Curricula.Add(baseCurriculum);
            }
        }
    }
}

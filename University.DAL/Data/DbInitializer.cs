using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.DAL.Models;

namespace University.DAL.Data
{
    public static class DbInitializer
    {
        public static void Initialize(UniversityContext context)
        {
            context.Database.EnsureCreated();

            if (context.Courses.Any())
            {
                return; // DB has been seeded
            }

            var instructors = new List<Instructor>
            {
                new Instructor { Name = "Dr. Smith", Department = "Computer Science" },
                new Instructor { Name = "Prof. Johnson", Department = "Mathematics" },
                new Instructor { Name = "Dr. Brown", Department = "Physics" }
            };

            var courses = new List<Course>
            {
                new Course { Title = "Introduction to Programming", Schedule = "Mon/Wed 10-11 AM", Capacity = 30, RegisteredStudents = 0 },
                new Course { Title = "Calculus 101", Schedule = "Tue/Thu 2-3 PM", Capacity = 25, RegisteredStudents = 0 },
                new Course { Title = "Physics 101", Schedule = "Fri 1-3 PM", Capacity = 20, RegisteredStudents = 0 }
            };

            var students = new List<Student>
            {
                new Student { Name = "Alice" },
                new Student { Name = "Bob" },
                new Student { Name = "Charlie" },
                new Student { Name = "Diana" }
            };

            context.Instructors.AddRange(instructors);
            context.Courses.AddRange(courses);
            context.Students.AddRange(students);
            context.SaveChanges();

            // Establish relationships
            courses[0].Instructors.Add(instructors[0]); // Dr. Smith teaches Introduction to Programming
            courses[0].Instructors.Add(instructors[2]); // Dr. Brown also teaches Introduction to Programming
            courses[1].Instructors.Add(instructors[1]); // Prof. Johnson teaches Calculus 101
            courses[2].Instructors.Add(instructors[2]); // Dr. Brown teaches Physics 101

            students[0].EnrolledCourses.Add(courses[0]); // Alice enrolls in Introduction to Programming
            students[1].EnrolledCourses.Add(courses[1]); // Bob enrolls in Calculus 101
            students[2].EnrolledCourses.Add(courses[2]); // Charlie enrolls in Physics 101
            students[3].EnrolledCourses.Add(courses[0]); // Diana enrolls in Introduction to Programming

            context.SaveChanges();

            // Update RegisteredStudents for each course
            foreach (var course in courses)
            {
                course.RegisteredStudents = course.EnrolledStudents.Count;
            }

            context.SaveChanges();
        }
    }
}

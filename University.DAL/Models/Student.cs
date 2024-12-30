using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DAL.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentMail { get; set; }
        public ICollection<Course> EnrolledCourses { get; set; } = new List<Course>();

    }


}

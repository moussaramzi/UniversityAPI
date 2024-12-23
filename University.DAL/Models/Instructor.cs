using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DAL.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public ICollection<Course> CoursesTaught { get; set; } = new List<Course>();
    }
}

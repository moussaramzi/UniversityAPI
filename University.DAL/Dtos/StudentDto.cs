using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DAL.Dtos
{
    public class StudentDto
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public List<CourseDto> EnrolledCourses { get; set; } = new List<CourseDto>();
    }
}

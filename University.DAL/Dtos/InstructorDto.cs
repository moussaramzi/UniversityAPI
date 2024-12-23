using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DAL.Dtos
{
    public class InstructorDto
    {
        public int InstructorID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public List<CourseDto> CoursesTaught { get; set; } = new List<CourseDto>();
    }
}

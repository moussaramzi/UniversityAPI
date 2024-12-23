using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DAL.Dtos
{
   
    
        public class CourseDto
        {
            public int CourseID { get; set; }
            public string Title { get; set; }
            public string Schedule { get; set; }
            public int Capacity { get; set; }
            public int RegisteredStudents { get; set; }
            public List<InstructorDto> Instructors { get; set; } = new List<InstructorDto>();
            public List<StudentDto> EnrolledStudents { get; set; } = new List<StudentDto>();
        }
    
}

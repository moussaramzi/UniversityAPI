using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.DAL;
using University.DAL.Dtos;
using University.DAL.Models;

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository<Student> _studentRepository;

        public StudentsController(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _studentRepository.GetAsync(
                includes: s => s.EnrolledCourses
            );

            var studentDtos = students.Select(s => new StudentDto
            {
                StudentID = s.StudentID,
                Name = s.Name,
                EnrolledCourses = s.EnrolledCourses.Select(c => new CourseDto
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Schedule = c.Schedule
                }).ToList()
            });

            return Ok(studentDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            

            var student = await _studentRepository.GetAsync(
                filter: s => s.StudentID == id,
                includes: s => s.EnrolledCourses
            );

            var studentDto = student.Select(s => new StudentDto
            {
                StudentID = s.StudentID,
                Name = s.Name,
                EnrolledCourses = s.EnrolledCourses.Select(c => new CourseDto
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Schedule = c.Schedule
                }).ToList()
            }).FirstOrDefault();

            return Ok(studentDto);
        }
    }
}

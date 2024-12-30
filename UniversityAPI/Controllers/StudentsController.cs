using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.DAL;
using University.DAL.Models;

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Course> _courseRepository;

        public StudentsController(IRepository<Student> studentRepository, IRepository<Course> courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _studentRepository.GetAsync(
                includes: s => s.EnrolledCourses
            );
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _studentRepository.GetAsync(
                filter: s => s.StudentID == id,
                includes: s => s.EnrolledCourses
            );
            return Ok(student.FirstOrDefault());
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentRequest request)
        {
            var student = await _studentRepository.GetAsync(
                filter: s => s.StudentID == request.StudentId,
                includes: s => s.EnrolledCourses
            );

            if (student == null || !student.Any())
            {
                return NotFound(new { message = "Student not found" });
            }

            var course = await _courseRepository.GetAsync(
                filter: c => c.Title == request.CourseTitle
            );

            if (course == null || !course.Any())
            {
                return NotFound(new { message = "Course not found" });
            }

            var studentEntity = student.First();
            var courseEntity = course.First();

            if (studentEntity.EnrolledCourses.Any(c => c.CourseID == courseEntity.CourseID))
            {
                return Conflict(new { message = "Student is already enrolled in this course" });
            }

            studentEntity.EnrolledCourses.Add(courseEntity);
            courseEntity.RegisteredStudents++;

            await _studentRepository.UpdateAsync(studentEntity);

            return Ok(new { message = $"Student {studentEntity.FirstName} successfully enrolled in {courseEntity.Title}" });
        }
    }
}

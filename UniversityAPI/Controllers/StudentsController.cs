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
    // Check if the student exists
    var student = await _studentRepository.GetAsync(
        filter: s => s.StudentID == request.StudentId,
        includes: s => s.EnrolledCourses
    );

    Student studentEntity;

    if (student == null || !student.Any())
    {
        // Create a new student if not found
        studentEntity = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            StudentMail = request.StudentMail,
        };
        
        _studentRepository.Insert(studentEntity);
        
        // Save changes to ensure StudentID is generated
        await _studentRepository.SaveChangesAsync();
    }
    else
    {
        studentEntity = student.First();
    }

    var errors = new List<string>();
    var successes = new List<string>();

    // Enroll the student in the specified courses
    foreach (var courseTitle in request.CourseTitles)
    {
        var course = await _courseRepository.GetAsync(filter: c => c.Title == courseTitle);

        if (course == null || !course.Any())
        {
            errors.Add($"Course '{courseTitle}' not found");
            continue;
        }

        var courseEntity = course.First();

        if (studentEntity.EnrolledCourses.Any(c => c.CourseID == courseEntity.CourseID))
        {
            errors.Add($"Student is already enrolled in '{courseTitle}'");
            continue;
        }

        studentEntity.EnrolledCourses.Add(courseEntity);
        courseEntity.RegisteredStudents++;
        successes.Add(courseTitle);
    }

    // Update the student with the new enrollments
    await _studentRepository.UpdateAsync(studentEntity);

    return Ok(new
    {
        message = "Enrollment completed",
        successes,
        errors
    });
}


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.DAL.Data;
using University.DAL.Dtos;
using University.DAL.Models;

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityContext _context;

        public StudentsController(UniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.EnrolledCourses)
                .Select(s => new StudentDto
                {
                    StudentID = s.StudentID,
                    Name = s.Name,
                    EnrolledCourses = s.EnrolledCourses.Select(c => new CourseDto
                    {
                        CourseID = c.CourseID,
                        Title = c.Title,
                        Schedule = c.Schedule
                    }).ToList()
                }).ToListAsync();

            return Ok(students);
        }
    

    private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}

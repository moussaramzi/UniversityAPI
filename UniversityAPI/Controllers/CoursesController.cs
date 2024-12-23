using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.DAL.Data;
using University.DAL.Models;
using University.DAL.Dtos;

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly UniversityContext _context;

        public CoursesController(UniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.EnrolledStudents)
                .Include(c => c.Instructors)
                .Select(c => new CourseDto
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Schedule = c.Schedule,
                    Capacity = c.Capacity,
                    RegisteredStudents = c.RegisteredStudents,
                    Instructors = c.Instructors.Select(i => new InstructorDto
                    {
                        InstructorID = i.InstructorID,
                        Name = i.Name,
                        Department = i.Department
                    }).ToList(),
                    EnrolledStudents = c.EnrolledStudents.Select(s => new StudentDto
                    {
                        StudentID = s.StudentID,
                        Name = s.Name
                    }).ToList()
                }).ToListAsync();

            return Ok(courses);
        }
    

    private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}

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
    public class InstructorsController : ControllerBase
    {
        private readonly UniversityContext _context;

        public InstructorsController(UniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorDto>>> GetInstructors()
        {
            var instructors = await _context.Instructors
                .Include(i => i.CoursesTaught)
                .Select(i => new InstructorDto
                {
                    InstructorID = i.InstructorID,
                    Name = i.Name,
                    Department = i.Department,
                    CoursesTaught = i.CoursesTaught.Select(c => new CourseDto
                    {
                        CourseID = c.CourseID,
                        Title = c.Title
                    }).ToList()
                }).ToListAsync();

            return Ok(instructors);
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorID == id);
        }
    }
}

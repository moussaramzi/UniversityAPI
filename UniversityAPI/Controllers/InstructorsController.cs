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
    public class InstructorsController : ControllerBase
    {
        private readonly IRepository<Instructor> _instructorRepository;

        public InstructorsController(IRepository<Instructor> instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorDto>>> GetInstructors()
        {
            var instructors = await _instructorRepository.GetAsync(
                includes: i => i.CoursesTaught
            );

            var instructorDtos = instructors.Select(i => new InstructorDto
            {
                InstructorID = i.InstructorID,
                Name = i.Name,
                Department = i.Department,
                CoursesTaught = i.CoursesTaught.Select(c => new CourseDto
                {
                    CourseID = c.CourseID,
                    Title = c.Title
                }).ToList()
            });

            return Ok(instructorDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDto>> GetInstructorById(int id)
        {
            

            var instructor = await _instructorRepository.GetAsync(
                filter: i => i.InstructorID == id,
                includes: i => i.CoursesTaught
            );

            var instructorDto = instructor.Select(i => new InstructorDto
            {
                InstructorID = i.InstructorID,
                Name = i.Name,
                Department = i.Department,
                CoursesTaught = i.CoursesTaught.Select(c => new CourseDto
                {
                    CourseID = c.CourseID,
                    Title = c.Title
                }).ToList()
            }).FirstOrDefault();

            return Ok(instructorDto);
        }
    }
}

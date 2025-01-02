using Microsoft.AspNetCore.Mvc;
using University.DAL.Models;
using University.DAL;

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IRepository<Event> _eventRepository;

        public EventController(IRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _eventRepository.GetAsync(null, null, c => c.eventName);

            if (events == null || !events.Any())
            {
                return NotFound("No events found.");
            }

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var events = await _eventRepository.GetByIDAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }
    }

}

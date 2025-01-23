using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Models;
using ProScore.Api.Services;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Event/{matchId}
        [HttpGet("{matchId}")]
        public IActionResult GetEventsByMatch(int matchId)
        {
            var events = _eventService.GetEventsByMatch(matchId);

            if (!events.Any()) return NotFound("Nenhum evento encontrado para esta partida.");
            return Ok(events);
        }

        // POST: api/Event
        [HttpPost]
        public IActionResult CreateEvent(Event gameEvent)
        {
            try
            {
                var createdEvent = _eventService.CreateEvent(gameEvent);
                return CreatedAtAction(nameof(GetEventsByMatch), new { matchId = createdEvent.MatchId }, createdEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Event/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, Event updatedEvent)
        {
            var success = _eventService.UpdateEvent(id, updatedEvent);
            if (!success) return NotFound("Evento não encontrado.");
            return NoContent();
        }

        // DELETE: api/Event/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var success = _eventService.DeleteEvent(id);
            if (!success) return NotFound("Evento não encontrado.");
            return NoContent();
        }
    }
}

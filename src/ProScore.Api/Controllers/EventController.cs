using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProScore.Api.Data;
using ProScore.Api.Models;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ProScoreContext _context;

        public EventController(ProScoreContext context)
        {
            _context = context;
        }

        // GET: api/Event/{matchId}
        [HttpGet("{matchId}")]
        public IActionResult GetEventsByMatch(int matchId)
        {
            var events = _context.Events
                .Include(e => e.Player)
                .Where(e => e.MatchId == matchId)
                .ToList();

            if (!events.Any()) return NotFound("Nenhum evento encontrado para esta partida.");
            return Ok(events);
        }

        // POST: api/Event
        [HttpPost]
        public IActionResult CreateEvent(Event gameEvent)
        {
            var match = _context.Matches.Find(gameEvent.MatchId);
            var player = _context.Players.Find(gameEvent.PlayerId);

            if (match == null) return NotFound("Partida não encontrada.");
            if (player == null) return NotFound("Jogador não encontrado.");

            _context.Events.Add(gameEvent);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEventsByMatch), new { matchId = gameEvent.MatchId }, gameEvent);
        }

        // PUT: api/Event/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, Event updatedEvent)
        {
            var gameEvent = _context.Events.Find(id);
            if (gameEvent == null) return NotFound("Evento não encontrado.");

            gameEvent.Type = updatedEvent.Type;
            gameEvent.Minute = updatedEvent.Minute;
            gameEvent.PlayerId = updatedEvent.PlayerId;
            gameEvent.MatchId = updatedEvent.MatchId;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Event/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var gameEvent = _context.Events.Find(id);
            if (gameEvent == null) return NotFound("Evento não encontrado.");

            _context.Events.Remove(gameEvent);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

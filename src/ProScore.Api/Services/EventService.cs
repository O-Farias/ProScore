using ProScore.Api.Data;
using ProScore.Api.Models;

namespace ProScore.Api.Services
{
    public class EventService
    {
        private readonly ProScoreContext _context;

        public EventService(ProScoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetEventsByMatch(int matchId)
        {
            return _context.Events
                .Include(e => e.Player)
                .Where(e => e.MatchId == matchId)
                .ToList();
        }

        public Event? CreateEvent(Event gameEvent)
        {
            var match = _context.Matches.Find(gameEvent.MatchId);
            if (match == null) throw new Exception("Partida não encontrada.");

            var player = _context.Players.Find(gameEvent.PlayerId);
            if (player == null) throw new Exception("Jogador não encontrado.");

            _context.Events.Add(gameEvent);
            _context.SaveChanges();
            return gameEvent;
        }

        public bool UpdateEvent(int id, Event updatedEvent)
        {
            var gameEvent = _context.Events.Find(id);
            if (gameEvent == null) return false;

            gameEvent.Type = updatedEvent.Type;
            gameEvent.Minute = updatedEvent.Minute;
            gameEvent.PlayerId = updatedEvent.PlayerId;
            gameEvent.MatchId = updatedEvent.MatchId;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteEvent(int id)
        {
            var gameEvent = _context.Events.Find(id);
            if (gameEvent == null) return false;

            _context.Events.Remove(gameEvent);
            _context.SaveChanges();
            return true;
        }
    }
}

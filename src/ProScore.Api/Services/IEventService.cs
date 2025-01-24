using ProScore.Api.Models;

namespace ProScore.Api.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetEventsByMatch(int matchId);
        Event? GetEventById(int id);
        Event CreateEvent(Event gameEvent);
        bool UpdateEvent(int id, Event gameEvent);
        bool DeleteEvent(int id);
    }
}
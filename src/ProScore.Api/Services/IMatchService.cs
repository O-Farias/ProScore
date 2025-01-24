using ProScore.Api.Models;

namespace ProScore.Api.Services
{
    public interface IMatchService
    {
        IEnumerable<Match> GetAllMatches();
        Match? GetMatchById(int id);  
        Match CreateMatch(Match match);
        bool UpdateMatch(int id, Match match);
        bool DeleteMatch(int id);
    }
}
using ProScore.Api.Data;
using ProScore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ProScore.Api.Services
{
    public class MatchService
    {
        private readonly ProScoreContext _context;

        public MatchService(ProScoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Match> GetAllMatches()
        {
            return _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToList();
        }

        public Match? GetMatchById(int id)
        {
            return _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .FirstOrDefault(m => m.Id == id);
        }

        public Match? CreateMatch(Match match)
        {
            var homeTeam = _context.Teams.Find(match.HomeTeamId);
            var awayTeam = _context.Teams.Find(match.AwayTeamId);

            if (homeTeam == null || awayTeam == null)
                throw new Exception("Um ou ambos os times não foram encontrados.");

            _context.Matches.Add(match);
            _context.SaveChanges();
            return match;
        }

        public bool UpdateMatch(int id, Match updatedMatch)
        {
            var match = _context.Matches.Find(id);
            if (match == null) return false;

            match.Date = updatedMatch.Date;
            match.Location = updatedMatch.Location;
            match.HomeTeamId = updatedMatch.HomeTeamId;
            match.AwayTeamId = updatedMatch.AwayTeamId;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteMatch(int id)
        {
            var match = _context.Matches.Find(id);
            if (match == null) return false;

            _context.Matches.Remove(match);
            _context.SaveChanges();
            return true;
        }
    }
}

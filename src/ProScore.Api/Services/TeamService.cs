using ProScore.Api.Data;
using ProScore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ProScore.Api.Services
{
    public class TeamService
    {
        private readonly ProScoreContext _context;

        public TeamService(ProScoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _context.Teams.ToList();
        }

        public Team? GetTeamById(int id)
        {
            return _context.Teams.Find(id);
        }

        public Team CreateTeam(Team team)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();
            return team;
        }

        public bool UpdateTeam(int id, Team updatedTeam)
        {
            var team = _context.Teams.Find(id);
            if (team == null) return false;

            team.Name = updatedTeam.Name;
            team.Country = updatedTeam.Country;
            team.Founded = updatedTeam.Founded;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteTeam(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null) return false;

            _context.Teams.Remove(team);
            _context.SaveChanges();
            return true;
        }
    }
}

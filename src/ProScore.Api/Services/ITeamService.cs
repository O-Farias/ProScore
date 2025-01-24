using ProScore.Api.Models;

namespace ProScore.Api.Services
{
    public interface ITeamService
    {
        IEnumerable<Team> GetAllTeams();
        Team GetTeamById(int id);
        Team CreateTeam(Team team);
        bool UpdateTeam(int id, Team team);
        bool DeleteTeam(int id);
    }
}
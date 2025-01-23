using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Models;
using ProScore.Api.Services;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly TeamService _teamService;

        public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public IActionResult GetAllTeams()
        {
            var teams = _teamService.GetAllTeams();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public IActionResult GetTeamById(int id)
        {
            var team = _teamService.GetTeamById(id);
            if (team == null) return NotFound("Time não encontrado.");
            return Ok(team);
        }

        [HttpPost]
        public IActionResult CreateTeam(Team team)
        {
            var createdTeam = _teamService.CreateTeam(team);
            return CreatedAtAction(nameof(GetTeamById), new { id = createdTeam.Id }, createdTeam);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeam(int id, Team updatedTeam)
        {
            var success = _teamService.UpdateTeam(id, updatedTeam);
            if (!success) return NotFound("Time não encontrado.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            var success = _teamService.DeleteTeam(id);
            if (!success) return NotFound("Time não encontrado.");
            return NoContent();
        }
    }
}

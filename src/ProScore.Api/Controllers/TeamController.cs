using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Data;
using ProScore.Api.Models;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ProScoreContext _context;

        public TeamController(ProScoreContext context)
        {
            _context = context;
        }

        // GET: api/Team
        [HttpGet]
        public IActionResult GetAllTeams()
        {
            var teams = _context.Teams.ToList();
            return Ok(teams);
        }

        // GET: api/Team/{id}
        [HttpGet("{id}")]
        public IActionResult GetTeamById(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null) return NotFound("Time não encontrado.");
            return Ok(team);
        }

        // POST: api/Team
        [HttpPost]
        public IActionResult CreateTeam(Team team)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
        }

        // PUT: api/Team/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTeam(int id, Team updatedTeam)
        {
            var team = _context.Teams.Find(id);
            if (team == null) return NotFound("Time não encontrado.");

            team.Name = updatedTeam.Name;
            team.Country = updatedTeam.Country;
            team.Founded = updatedTeam.Founded;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Team/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null) return NotFound("Time não encontrado.");

            _context.Teams.Remove(team);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

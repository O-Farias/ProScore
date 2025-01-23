using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Data;
using ProScore.Api.Models;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly ProScoreContext _context;

        public MatchController(ProScoreContext context)
        {
            _context = context;
        }

        // GET: api/Match
        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToList();

            return Ok(matches);
        }

        // GET: api/Match/{id}
        [HttpGet("{id}")]
        public IActionResult GetMatchById(int id)
        {
            var match = _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .FirstOrDefault(m => m.Id == id);

            if (match == null) return NotFound("Partida não encontrada.");
            return Ok(match);
        }

        // POST: api/Match
        [HttpPost]
        public IActionResult CreateMatch(Match match)
        {
            var homeTeam = _context.Teams.Find(match.HomeTeamId);
            var awayTeam = _context.Teams.Find(match.AwayTeamId);

            if (homeTeam == null || awayTeam == null)
                return BadRequest("Um ou ambos os times não foram encontrados.");

            _context.Matches.Add(match);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetMatchById), new { id = match.Id }, match);
        }

        // PUT: api/Match/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateMatch(int id, Match updatedMatch)
        {
            var match = _context.Matches.Find(id);
            if (match == null) return NotFound("Partida não encontrada.");

            match.Date = updatedMatch.Date;
            match.Location = updatedMatch.Location;
            match.HomeTeamId = updatedMatch.HomeTeamId;
            match.AwayTeamId = updatedMatch.AwayTeamId;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Match/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteMatch(int id)
        {
            var match = _context.Matches.Find(id);
            if (match == null) return NotFound("Partida não encontrada.");

            _context.Matches.Remove(match);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Models;
using ProScore.Api.Services;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;

        public MatchController(MatchService matchService)
        {
            _matchService = matchService;
        }

        // GET: api/Match
        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _matchService.GetAllMatches();
            return Ok(matches);
        }

        // GET: api/Match/{id}
        [HttpGet("{id}")]
        public IActionResult GetMatchById(int id)
        {
            var match = _matchService.GetMatchById(id);
            if (match == null) return NotFound("Partida não encontrada.");
            return Ok(match);
        }

        // POST: api/Match
        [HttpPost]
        public IActionResult CreateMatch(Match match)
        {
            try
            {
                var createdMatch = _matchService.CreateMatch(match);
                return CreatedAtAction(nameof(GetMatchById), new { id = createdMatch.Id }, createdMatch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Match/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateMatch(int id, Match updatedMatch)
        {
            var success = _matchService.UpdateMatch(id, updatedMatch);
            if (!success) return NotFound("Partida não encontrada.");
            return NoContent();
        }

        // DELETE: api/Match/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteMatch(int id)
        {
            var success = _matchService.DeleteMatch(id);
            if (!success) return NotFound("Partida não encontrada.");
            return NoContent();
        }
    }
}

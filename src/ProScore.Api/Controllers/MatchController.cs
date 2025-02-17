using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Models;
using ProScore.Api.Services;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _matchService.GetAllMatches();
            if (!matches.Any()) return NotFound("Nenhuma partida encontrada.");
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public IActionResult GetMatchById(int id)
        {
            var match = _matchService.GetMatchById(id);
            if (match == null) return NotFound("Partida não encontrada.");
            return Ok(match);
        }

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

        [HttpPut("{id}")]
        public IActionResult UpdateMatch(int id, Match match)
        {
            var success = _matchService.UpdateMatch(id, match);
            if (!success) return NotFound("Partida não encontrada.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMatch(int id)
        {
            var success = _matchService.DeleteMatch(id);
            if (!success) return NotFound("Partida não encontrada.");
            return NoContent();
        }
    }
}
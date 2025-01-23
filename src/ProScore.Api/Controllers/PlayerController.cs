using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Data;
using ProScore.Api.Models;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ProScoreContext _context;

        public PlayerController(ProScoreContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            var players = _context.Players.ToList();
            return Ok(players);
        }

        // GET: api/Player/{id}
        [HttpGet("{id}")]
        public IActionResult GetPlayerById(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null) return NotFound("Jogador não encontrado.");
            return Ok(player);
        }

        // POST: api/Player
        [HttpPost]
        public IActionResult CreatePlayer(Player player)
        {
            var team = _context.Teams.Find(player.TeamId);
            if (team == null) return NotFound("Time não encontrado para associar o jogador.");

            _context.Players.Add(player);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player);
        }

        // PUT: api/Player/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, Player updatedPlayer)
        {
            var player = _context.Players.Find(id);
            if (player == null) return NotFound("Jogador não encontrado.");

            player.Name = updatedPlayer.Name;
            player.Number = updatedPlayer.Number;
            player.Position = updatedPlayer.Position;
            player.TeamId = updatedPlayer.TeamId;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Player/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null) return NotFound("Jogador não encontrado.");

            _context.Players.Remove(player);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

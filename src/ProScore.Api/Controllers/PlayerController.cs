using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET: api/Player
        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            var players = _playerService.GetAllPlayers();
            return Ok(players);
        }

        // GET: api/Player/{id}
        [HttpGet("{id}")]
        public IActionResult GetPlayerById(int id)
        {
            var player = _playerService.GetPlayerById(id);
            if (player == null) return NotFound("Jogador não encontrado.");
            return Ok(player);
        }

        // POST: api/Player
        [HttpPost]
        public IActionResult CreatePlayer(Player player)
        {
            try
            {
                var createdPlayer = _playerService.CreatePlayer(player);
                return CreatedAtAction(nameof(GetPlayerById), new { id = createdPlayer.Id }, createdPlayer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Player/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, Player updatedPlayer)
        {
            var success = _playerService.UpdatePlayer(id, updatedPlayer);
            if (!success) return NotFound("Jogador não encontrado.");
            return NoContent();
        }

        // DELETE: api/Player/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            var success = _playerService.DeletePlayer(id);
            if (!success) return NotFound("Jogador não encontrado.");
            return NoContent();
        }
    }
}
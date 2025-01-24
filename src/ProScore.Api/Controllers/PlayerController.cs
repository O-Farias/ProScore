using Microsoft.AspNetCore.Mvc;
using ProScore.Api.Models;
using ProScore.Api.Services;

namespace ProScore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            var players = _playerService.GetAllPlayers();
            return Ok(players);
        }

        [HttpGet("{id}")]
        public IActionResult GetPlayerById(int id)
        {
            var player = _playerService.GetPlayerById(id);
            if (player == null) return NotFound("Jogador não encontrado.");
            return Ok(player);
        }

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

        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, Player updatedPlayer)
        {
            var success = _playerService.UpdatePlayer(id, updatedPlayer);
            if (!success) return NotFound("Jogador não encontrado.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            var success = _playerService.DeletePlayer(id);
            if (!success) return NotFound("Jogador não encontrado.");
            return NoContent();
        }
    }
}
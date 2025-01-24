using ProScore.Api.Data;
using ProScore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ProScore.Api.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ProScoreContext _context;

        public PlayerService(ProScoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }

        public Player? GetPlayerById(int id)
        {
            return _context.Players.Find(id);
        }

        public Player CreatePlayer(Player player)
        {
            var team = _context.Teams.Find(player.TeamId);
            if (team == null) throw new Exception("Time não encontrado para associar o jogador.");

            _context.Players.Add(player);
            _context.SaveChanges();
            return player;
        }

        public bool UpdatePlayer(int id, Player updatedPlayer)
        {
            var player = _context.Players.Find(id);
            if (player == null) return false;

            player.Name = updatedPlayer.Name;
            player.Number = updatedPlayer.Number;
            player.Position = updatedPlayer.Position;
            player.TeamId = updatedPlayer.TeamId;

            _context.SaveChanges();
            return true;
        }

        public bool DeletePlayer(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null) return false;

            _context.Players.Remove(player);
            _context.SaveChanges();
            return true;
        }
    }
}
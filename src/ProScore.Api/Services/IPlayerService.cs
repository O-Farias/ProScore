using ProScore.Api.Models;

namespace ProScore.Api.Services
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetAllPlayers();
        Player GetPlayerById(int id);
        Player CreatePlayer(Player player);
        bool UpdatePlayer(int id, Player player);
        bool DeletePlayer(int id);
    }
}
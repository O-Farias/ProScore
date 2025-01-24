using Microsoft.EntityFrameworkCore;
using ProScore.Api.Models;

namespace ProScore.Api.Data
{
    public class ProScoreContext : DbContext
    {
        public ProScoreContext(DbContextOptions<ProScoreContext> options) : base(options)
        {
        }

        // Torna as propriedades virtuais para permitir mock
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Event> Events { get; set; }
    }
}
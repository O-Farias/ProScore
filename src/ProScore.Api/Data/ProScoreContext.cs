using Microsoft.EntityFrameworkCore;
using ProScore.Api.Models;

namespace ProScore.Api.Data
{
    public class ProScoreContext : DbContext
    {
        public ProScoreContext(DbContextOptions<ProScoreContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
    }
}

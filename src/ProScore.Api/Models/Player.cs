namespace ProScore.Api.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Position { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}

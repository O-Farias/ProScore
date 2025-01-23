namespace ProScore.Api.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;
        public string Type { get; set; } = string.Empty; 
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int Minute { get; set; }
    }
}

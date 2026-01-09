namespace CloudGamesStore.Domain.Entities
{
    public class GameLibrary
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public int GameId { get; set; }
        public Game? Game { get; set; }
        public DateTime AcquiredDate { get; set; } = DateTime.UtcNow;
    }
}

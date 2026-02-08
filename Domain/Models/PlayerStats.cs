namespace Domain.Models;

public class PlayerStats
{
    public long MatchId { get; set; }
    public long UserId { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }

    public Match Match { get; set; }
    public User User { get; set; }
}

namespace Application.DTOs;

public class AddPlayerStatsDto
{
    public long MatchId { get; set; }
    public long UserId { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
}

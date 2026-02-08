namespace Application.DTOs;

public class UpdatePlayerStatsDto
{
    public long MatchId { get; set; }
    public long UserId { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
}

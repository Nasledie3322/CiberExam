namespace Application.DTOs;

public class UpdateMatchResultDto
{
    public long MatchId { get; set; }
    public long TeamId { get; set; }
    public int Score { get; set; }
}

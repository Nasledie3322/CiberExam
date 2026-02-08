namespace Domain.Models;

public class MatchResult
{
    public long MatchId { get; set; }
    public long TeamId { get; set; }
    public int Score { get; set; }

    public Match Match { get; set; }
    public Team Team { get; set; }
}

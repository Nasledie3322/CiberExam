namespace Domain.Models;

public class Match
{
    public long Id { get; set; }
    public long TournamentId { get; set; }
    public long TeamAId { get; set; }
    public long TeamBId { get; set; }
    public long WinnerTeamId { get; set; }
    public DateTime PlayedAt { get; set; }

    public Tournament Tournament { get; set; }
    public Team TeamA { get; set; }
    public Team TeamB { get; set; }
    public Team WinnerTeam { get; set; }
    public ICollection<MatchResult> MatchResults { get; set; } = new List<MatchResult>();
    public ICollection<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>();
}

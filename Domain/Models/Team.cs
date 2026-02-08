namespace Domain.Models;

public class Team
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long CaptainId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User Captain { get; set; }
    public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    public ICollection<Match> MatchesAsTeamA { get; set; } = new List<Match>();
    public ICollection<Match> MatchesAsTeamB { get; set; } = new List<Match>();
    public ICollection<MatchResult> MatchResults { get; set; } = new List<MatchResult>();
}

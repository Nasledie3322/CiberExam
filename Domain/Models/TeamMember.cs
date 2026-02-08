namespace Domain.Models;

public class TeamMember
{
    public long TeamId { get; set; }
    public long UserId { get; set; }
    public DateTime JoinedAt { get; set; }

    public Team Team { get; set; }
    public User User { get; set; }
}

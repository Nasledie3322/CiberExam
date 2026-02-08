namespace Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    public ICollection<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>();
}

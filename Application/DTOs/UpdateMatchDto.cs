namespace Application.DTOs;

public class UpdateMatchDto
{
    public long TournamentId { get; set; }
    public long TeamAId { get; set; }
    public long TeamBId { get; set; }
    public long WinnerTeamId { get; set; }
}

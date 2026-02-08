public class MatchDto
{
    public long Id { get; set; }
    public long TournamentId { get; set; }
    public long TeamAId { get; set; }
    public long TeamBId { get; set; }
    public long WinnerTeamId { get; set; }
    public DateTime PlayedAt { get; set; }
}

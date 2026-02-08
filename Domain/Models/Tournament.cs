namespace Domain.Models;

public class Tournament
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string GameName { get; set; }
    public DateTime StartDate { get; set; }
    public string Status { get; set; }

    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

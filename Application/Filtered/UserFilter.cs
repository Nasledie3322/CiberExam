namespace Application.Filtering;

public class UserFilter : PagedQuery
{
    public string? Username { get; set; }
    public string? Email { get; set; }
}

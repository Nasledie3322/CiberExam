namespace Application.DTOs;

public class UserFilterDto
{
    public string? Nickname { get; set; }
    public string? Email { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

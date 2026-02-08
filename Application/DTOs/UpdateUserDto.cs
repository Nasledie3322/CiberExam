namespace Application.DTOs;

public class UpdateUserDto
{
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}

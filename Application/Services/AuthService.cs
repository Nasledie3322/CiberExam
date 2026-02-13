using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(IJwtService jwtService, UserManager<ApplicationUser> userManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
    }

    public async Task<string> Register(string username, string password)
    {
        var user = new ApplicationUser { UserName = username };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return _jwtService.GenerateToken(user.Id, username);
    }

    public async Task<string> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) throw new Exception("Polzovatel ne nayden");

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid) throw new Exception("Neverniy parol");

        return _jwtService.GenerateToken(user.Id, username);
    }
}

using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<string> Login(string username, string password);
    Task<string> Register(string username, string password, string role);
}

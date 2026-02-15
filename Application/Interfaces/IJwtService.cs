using System.Threading.Tasks;
using Infrastructure.Data;

namespace Application.Interfaces;

public interface IJwtService
{
    Task<string> GenerateToken(ApplicationUser user);
}

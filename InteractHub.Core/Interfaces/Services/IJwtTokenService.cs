using InteractHub.Core.Entities;

namespace InteractHub.Core.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user, IList<string> roles);
}
using AuthPlayground.Domain.Entities.Identity;

namespace AuthPlayground.Application.Common.Security;

public interface ITokenService
{
    Task<TokenDto> CreateTokenAsync(AppUser user);
    string CreateRefreshToken();
}

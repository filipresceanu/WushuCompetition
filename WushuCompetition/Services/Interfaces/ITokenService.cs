using Microsoft.AspNetCore.Identity;
using WushuCompetition.Helper;

namespace WushuCompetition.Services.Interfaces
{
    public interface ITokenService
    {
        Task<AuthResult> CreateToken(IdentityUser user);
        Task<AuthResult> GenerateRefreshToken(TokenRequest tokenRequest);
        Task<AuthResult> VerifyToken(TokenRequest tokenRequest);
    }
}

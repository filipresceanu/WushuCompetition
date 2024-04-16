using WushuCompetition.Helper;
using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshToken(TokenRequest tokenRequest);
        Task UpdateRefreshToken(RefreshToken refreshToken);
    }
}

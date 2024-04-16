using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Helper;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DataContext _dataContext;

        public RefreshTokenRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateRefreshToken(RefreshToken refreshToken)
        {
            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshToken(TokenRequest tokenRequest)
        {
            var refreshToken =
                await _dataContext.RefreshTokens.FirstOrDefaultAsync(elem => elem.Token == tokenRequest.RefreshToken);

            return refreshToken;

        }

        public async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            var token = await _dataContext.RefreshTokens.FirstOrDefaultAsync(elem => elem.Token == refreshToken.Token);
            token.IsUsed = refreshToken.IsUsed;
            await _dataContext.SaveChangesAsync();
        }
    }
}

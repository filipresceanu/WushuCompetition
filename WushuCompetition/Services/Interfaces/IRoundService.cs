using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Services.Interfaces
{
    public interface IRoundService
    {
        Task<IEnumerable<RoundDto>> GetRoundsForSpecificRefereeNoWinner(string refereeId);

        Task CreateRoundsForMatches(Guid matchId);

    }
}

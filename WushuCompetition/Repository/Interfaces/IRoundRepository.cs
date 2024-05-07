using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IRoundRepository
    {
        Task CreateRoundsForMatches(RoundDto roundDto);
        Task<IEnumerable<RoundDto>> GetRoundsForSpecificMatchNoWinner(Guid matchId);
        Task<Round> GetRoundByIdNoWinner(Guid roundId);
        Task<Round> AddPointsInRoundNoWinner(Guid roundId, int pointsFirstParticipants,
            int pointsSecondParticipants);
        Task<RoundDto> CalculateWinner(Guid roundId);
        Task<IEnumerable<RoundDto>> GetRoundsWithMatchId(Guid matchId);
    }
}

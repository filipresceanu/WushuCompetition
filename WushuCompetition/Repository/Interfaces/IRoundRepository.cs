using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IRoundRepository
    {
        Task CreateRoundForMatch(RoundDto roundDto);
        Task<IEnumerable<RoundDto>> GetRoundsForSpecificMatchNoWinner(Guid matchId);
        Task<Round> GetRoundByIdNoWinner(Guid roundId);
        Task<Round> AddPointsInRoundNoWinner(Guid roundId, int pointsFirstParticipants,
            int pointsSecondParticipants);
        Task<RoundDto> CalculateWinnerRound(Guid roundId);
        Task<IEnumerable<RoundDto>> GetRoundsWithMatchId(Guid matchId);
    }
}

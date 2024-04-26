using WushuCompetition.Dto;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IRoundRepository
    {
        Task CreateRoundsForMatches(RoundDto roundDto);
        Task<IEnumerable<RoundDto>> GetRoundsForSpecificMatchNoWinner(Guid matchId);
        Task<RoundDto> GetRoundByIdNoWinner(Guid roundId);
        Task<RoundDto> AddPointsInRoundNoWinner(Guid roundId, int pointsFirstParticipants,
            int pointsSecondParticipants);
    }
}

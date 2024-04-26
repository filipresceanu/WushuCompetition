using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IMatchRepository
    {
        Task CreateMatch(Participant participantFirst, Participant participantSecond);
        Task<IEnumerable<MatchDto>> GetNumberOfMatchesNoReferee();
        Task AddRefereeInMatches(Guid matchId, string refereeId);
    }
}

using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IMatchRepository
    {
        Task CreateMatch(Participant participantFirst, Participant participantSecond);
        Task<IEnumerable<MatchDto>> GetNumberOfMatchesNoReferee();
        Task AddRefereeInMatches(Guid matchId, string refereeId);
        Task<IEnumerable<MatchDto>> GetMatchesForRefereeNoWinner(string refereeId);
        Task<Match> GetMatchWithId(Guid matchId);
        Task SetWinnerInMatch(Guid matchId, Guid winnerId);
    }
}

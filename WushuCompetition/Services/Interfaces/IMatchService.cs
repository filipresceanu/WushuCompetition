using WushuCompetition.Dto;

namespace WushuCompetition.Services.Interfaces
{
    public interface IMatchService
    {
        Task HandleParticipantsNumber(Guid competitionId);
        Task<MatchResultDto> CalculateWinnerMatch(Guid matchId);


    }
}

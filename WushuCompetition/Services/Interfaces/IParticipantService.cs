using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<string> AddParticipantsInCompetition(Guid competitionId, ParticipantDto participantDto);

        Task<IEnumerable<ParticipantDto>> GetParticipantsInCompetitionId(Guid competitionId);

        Task<IEnumerable<Participant>> GetParticipantsDataInCompetitionId(Guid competition);

        Task<IEnumerable<Participant>> GetParticipantsRandomCategoryAndCompetition(Guid categoryId, Guid competitionId);
        Task<IEnumerable<Participant>> GetParticipantsWinnerRandomCategoryAndCompetition(Guid categoryId, Guid competitionId);

        IEnumerable<Participant> ShufflingParticipants(IEnumerable<Participant> participants);
        Task DeleteParticipant(Guid id);
    }
}

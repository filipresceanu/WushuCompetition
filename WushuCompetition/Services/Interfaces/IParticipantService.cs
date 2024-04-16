using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<string> AddParticipantsInCompetition(Guid competitionId, ParticipantDto participantDto);

        Task<IEnumerable<ParticipantDto>> GetParticipantsInCompetitionId(Guid competitionId);

        Task<IEnumerable<Participant>> GetParticipantsDataInCompetitionId(Guid competiton);

        Task<IEnumerable<Participant>> GetParticipantsRandomCategoyAndCompetition(Guid categoryId, Guid competitionId);

        IEnumerable<Participant> ShufflingParticipants(IEnumerable<Participant> participants);
        Task DeleteParticipant(Guid id);
    }
}

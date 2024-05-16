using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IParticipantRepository
    {
        Task AddParticipantsInCompetition(Participant participant);

        Task<IEnumerable<ParticipantDto>> GetParticipantsForCompetitionId(Guid competitionId);

        Task<IEnumerable<Participant>> GetParticipantsDataForCompetitionId(Guid competitionId);

        Task<IEnumerable<Participant>> GetParticipantsForCategoryAndCompetition(Guid categoryId, Guid competitionId);

        Task<IEnumerable<Participant>> GetParticipantsShuffling();
        Task<IEnumerable<Participant>> GetParticipantsWinnersForCategoryAndCompetition(Guid categoryId, Guid competitionId);
        Task DeleteParticipants(Guid participantId);

        Task<Participant> GetParticipant(Guid participantId);
        Task UpdateParticipantCompeteInNextMatch(Guid participantId, bool status);
        Task<string> GetParticipantName(Guid participantId);

        Task Save();

        Task<int> GetParticipantNumberForCategoryAndCompetition(Guid categoryId, Guid competitionId);

        Task<ParticipantDto> GetParticipantDto(Guid participantId);

        //TODO edit participant
        //TODO delete participant
    }
}

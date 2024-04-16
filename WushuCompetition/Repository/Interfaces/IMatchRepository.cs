using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IMatchRepository
    {
        Task AddParticipantsInMatch(Participant participantFirst, Participant participantSecond);
    }
}

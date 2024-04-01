using WushuCompetition.Models;

namespace WushuCompetition.Repository
{
    public interface IMatchRepository
    {
        Task AddParticipantsInMatch(Participant participantFirst,Participant participantSecond);
    }
}

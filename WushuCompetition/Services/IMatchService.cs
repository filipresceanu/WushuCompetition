namespace WushuCompetition.Services
{
    public interface IMatchService
    {
        Task HandleParticipantsNumber(Guid competitionId);
    }
}

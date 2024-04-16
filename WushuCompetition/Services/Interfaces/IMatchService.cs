namespace WushuCompetition.Services.Interfaces
{
    public interface IMatchService
    {
        Task HandleParticipantsNumber(Guid competitionId);
    }
}

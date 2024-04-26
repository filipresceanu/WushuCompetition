namespace WushuCompetition.Repository.Interfaces
{
    public interface IRoundRepository
    {
        Task CreateRoundsForMatches(Guid matchId);
    }
}

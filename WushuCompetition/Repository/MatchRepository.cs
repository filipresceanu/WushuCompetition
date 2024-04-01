using WushuCompetition.Data;
using WushuCompetition.Models;

namespace WushuCompetition.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private readonly DataContext _dataContext;

        public MatchRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddParticipantsInMatch(Participant participantFirst, Participant participantSecond)
        {
            var match = new Match();
            match.CompetitorFirstId = participantFirst.Id;
            match.CompetitorSecondId = participantSecond.Id;
            _dataContext.Matches.Add(match);
            await _dataContext.SaveChangesAsync();
        }
    }
}

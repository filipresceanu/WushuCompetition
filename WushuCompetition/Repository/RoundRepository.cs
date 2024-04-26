using WushuCompetition.Data;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Repository
{
    public class RoundRepository:IRoundRepository
    {
        private readonly DataContext _dataContext;

        public RoundRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateRoundsForMatches(Guid matchId)
        {
            try
            {
                var round = new Round();
                round.MatchId = matchId;
                _dataContext.Rounds.Add(round);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
           
        }



    }
}

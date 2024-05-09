using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Repository
{
    public class RoundRepository : IRoundRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public RoundRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task CreateRoundForMatch(RoundDto roundDto)
        {
            try
            {
                var round = _mapper.Map<Round>(roundDto);
                _dataContext.Rounds.Add(round);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }

        public async Task<IEnumerable<RoundDto>> GetRoundsForSpecificMatchNoWinner(Guid matchId)
        {
            var rounds = await _dataContext.Rounds
                .Where(elem => elem.ParticipantWinnerId == null
                && elem.MatchId == matchId).ToArrayAsync();
            var roundsDto = rounds.Select(round => _mapper.Map<RoundDto>(round));

            return roundsDto;
        }

        public async Task<Round> GetRoundByIdNoWinner(Guid roundId)
        {
            var round = await _dataContext.Rounds.FirstOrDefaultAsync(elem =>
                elem.ParticipantWinnerId == null && elem.Id == roundId);

            return round;
        }

        public async Task<Round> AddPointsInRoundNoWinner(Guid roundId, int pointsFirstParticipants, int pointsSecondParticipants)
        {
            var round = await GetRoundByIdNoWinner(roundId);

            round.PointParticipantFirst += pointsFirstParticipants;
            round.PointParticipantSecond += pointsSecondParticipants;

            await _dataContext.SaveChangesAsync();

            return round;
        }

        public async Task<RoundDto> CalculateWinnerRound(Guid roundId)
        {
            try
            {
                var round = await GetRoundByIdNoWinner(roundId);
                if (round.PointParticipantFirst > round.PointParticipantSecond)
                {
                    round.ParticipantWinnerId = round.CompetitorFirstId;
                }
                else if (round.PointParticipantSecond > round.PointParticipantFirst)
                {
                    round.ParticipantWinnerId = round.CompetitorSecondId;
                }
                else
                {
                    round.ParticipantWinnerId = null;
                }

                await _dataContext.SaveChangesAsync();
                var roundDto = _mapper.Map<RoundDto>(round);
                return roundDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<RoundDto>> GetRoundsWithMatchId(Guid matchId)
        {
            var rounds = await _dataContext.Rounds.Where(elem => elem.MatchId == matchId).ToListAsync();
            var roundDto = rounds.Select(round => _mapper.Map<RoundDto>(round));
            return roundDto;
        }
    }
}

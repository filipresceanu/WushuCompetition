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

        public async Task CreateRoundsForMatches(RoundDto roundDto)
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

        public async Task<RoundDto> GetRoundByIdNoWinner(Guid roundId)
        {
            var round = await _dataContext.Rounds.FirstOrDefaultAsync(elem =>
                elem.ParticipantWinnerId == null && elem.Id == roundId);

            var roundDto = _mapper.Map<RoundDto>(round);
            return roundDto;
        }

        public async Task<RoundDto> AddPointsInRoundNoWinner(Guid roundId, int pointsFirstParticipants, int pointsSecondParticipants)
        {
            var round = await _dataContext.Rounds.FirstOrDefaultAsync(elem =>
                elem.ParticipantWinnerId == null && elem.Id == roundId);

            round.PointParticipantFirst = pointsFirstParticipants;
            round.PointParticipantSecond = pointsSecondParticipants;

            await _dataContext.SaveChangesAsync();

            var roundDto = _mapper.Map<RoundDto>(round);
            return roundDto;
        }

    }
}

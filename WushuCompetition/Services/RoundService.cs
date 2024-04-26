using AutoMapper;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Services
{
    public class RoundService:IRoundService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly IMapper _mapper;

        public RoundService(IMatchRepository matchRepository, IRoundRepository roundRepository, IMapper mapper)
        {
            _matchRepository = matchRepository;
            _roundRepository = roundRepository;
            _mapper = mapper;
        }

        public async Task CreateRoundsForMatches(Guid matchId)
        {
            var roundDto = new RoundDto();
            var match = await _matchRepository.GetMatchWithId(matchId);
            roundDto.MatchId = matchId;
            roundDto.CompetitorFirstId = match.CompetitorFirstId;
            roundDto.CompetitorSecondId=match.CompetitorSecondId;

            await _roundRepository.CreateRoundsForMatches(roundDto);
        }

        public async Task<IEnumerable<RoundDto>> GetRoundsForSpecificRefereeNoWinner(string refereeId)
        {
            var matches = await _matchRepository.GetMatchesForRefereeNoWinner(refereeId);
            var rounds = new List<RoundDto>();
            foreach (var matchDto in matches)
            {
                var roundDtos = await _roundRepository.GetRoundsForSpecificMatchNoWinner(matchDto.Id);
                rounds.AddRange(roundDtos);
            }
            return rounds;
        }

    }
}

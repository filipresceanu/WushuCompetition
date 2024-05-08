using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Services
{
    public class MatchService : IMatchService
    {
        private readonly IParticipantService _participantService;
        private readonly IMatchRepository _matchRepository;
        private readonly ICategoryService _categoryService;
        private readonly IRoundService _roundService;
        private readonly IRoundRepository _roundRepository;
        private readonly IParticipantRepository _participantRepository;

        public MatchService(IParticipantService participantService, IMatchRepository matchRepository,
            ICategoryService categoryService, IRoundService roundService, IRoundRepository roundRepository, IParticipantRepository participantRepository)
        {
            _participantService = participantService;
            _matchRepository = matchRepository;
            _categoryService = categoryService;
            _roundService = roundService;
            _roundRepository = roundRepository;
            _participantRepository = participantRepository;
        }

        public async Task HandleParticipantsNumber(Guid competitionId)
        {
            var categories = await _categoryService.GetCategoriesForCompetitionId(competitionId);
            foreach (var category in categories)
            {
                var participants = await _participantService.GetParticipantsWinnerRandomCategoryAndCompetition(category.Id, competitionId);
                if (participants.Count() % 2 != 0)
                {
                    await AddOddParticipantsNumberInMatches(participants);
                }
                if (participants.Count() % 2 == 0)
                {
                    await AddParticipantsInMatches(participants);//Add participants in matches no restrictions
                }
                if (participants.Count() == 3)
                {
                    //Add participants in matches special case
                }
            }

        }

        public async Task CalculateWinnerMatch(Guid matchId)
        {
            var rounds = await _roundRepository.GetRoundsWithMatchId(matchId);
            if (rounds.Count() == 2)
            {
                await SetWinnerTwoRounds(rounds);
            }

            if (rounds.Count() == 3)
            {
                await SetWinnerTreeRounds(rounds);
            }
        }

        private async Task SetWinnerTwoRounds(IEnumerable<RoundDto> rounds)
        {
            var roundsWithWinners = rounds.Where(elem => elem.ParticipantWinnerId != null).ToList();

            if ((rounds.First().ParticipantWinnerId != rounds.Last().ParticipantWinnerId
                 && roundsWithWinners.Count() == 2) || roundsWithWinners.Count() == 0)
            {
                var roundDto = new RoundDto();
                roundDto.CompetitorFirstId = rounds.First().CompetitorFirstId;
                roundDto.CompetitorSecondId = rounds.First().CompetitorSecondId;
                roundDto.MatchId = rounds.First().MatchId;
                await _roundRepository.CreateRoundsForMatches(roundDto);
            }
            if (roundsWithWinners.Count() == 1)
            {
                await _matchRepository.SetWinnerInMatch(roundsWithWinners.First().MatchId,
                    (Guid)roundsWithWinners.First().ParticipantWinnerId);
            }
            if (roundsWithWinners.Count() == 2)
            {
                await _matchRepository.SetWinnerInMatch(rounds.First().MatchId,
                    (Guid)rounds.First().ParticipantWinnerId);
            }
        }

        private async Task SetWinnerTreeRounds(IEnumerable<RoundDto> roundsDtos)
        {
            Dictionary<Guid, int> winnerFrequency = new Dictionary<Guid, int>();

            foreach (var roundDto in roundsDtos)
            {
                if (!winnerFrequency.ContainsKey((Guid)roundDto.ParticipantWinnerId))
                {
                    winnerFrequency.Add((Guid)roundDto.ParticipantWinnerId, 0);
                }
                winnerFrequency[(Guid)roundDto.ParticipantWinnerId]++;
            }

            var winner = winnerFrequency.MaxBy(entry => entry.Value);
            var sortDictionary = from entry in winnerFrequency 
                orderby winnerFrequency.Values select entry;

            if (winner.Value == 0 || sortDictionary.Last().Value==sortDictionary.ElementAt(sortDictionary.Count()-1).Value)
            {
                var winnerId = await GetParticipantLowestWeight(roundsDtos.First().MatchId);
                await _matchRepository.SetWinnerInMatch(roundsDtos.First().MatchId, winnerId);
            }

            await _matchRepository.SetWinnerInMatch(roundsDtos.First().MatchId, winner.Key);
        }

        private async Task AddRoundsInMatches()
        {
            var matches = await _matchRepository.GetNumberOfMatchesNoReferee();
            foreach (var match in matches)
            {
                await AddTwoRoundInMatch(match.Id);
            }
        }

        ///<summary>
        /// in each step take first two participants and add these participants into a match
        /// remove these two participants from list
        /// </summary>
        private async Task AddParticipantsInMatches(IEnumerable<Participant> participants)
        {
            var participantsList = participants.ToList(); //create a copy of this list in order to remove elements for it
            while (participantsList.Any())
            {
                var participantFirst = participantsList.First();
                participantsList.Remove(participantFirst);
                var participantSecond = participantsList.First();
                participantsList.Remove(participantSecond);
                await _matchRepository.CreateMatch(participantFirst, participantSecond);
            }

            await AddRoundsInMatches();
        }

        /// <summary>
        /// First we get a list of shuffling participants
        /// Create a temporary list
        /// Remove first participant form that list (all participants are in a random position)
        /// now we have an even number of participants 
        /// </summary>
        private async Task AddOddParticipantsNumberInMatches(IEnumerable<Participant> participants)
        {
            var participantsList = _participantService.ShufflingParticipants(participants).ToList();
            var luckyParticipant = participantsList.First();
            participantsList.Remove(luckyParticipant);
            await AddParticipantsInMatches(participantsList);
        }

        private async Task AddOnlyThreeParticipantsInMatches(IEnumerable<Participant> participants)
        {

        }

        private async Task AddTwoRoundInMatch(Guid matchId)
        {
            await _roundService.CreateRoundsForMatches(matchId);
            await _roundService.CreateRoundsForMatches(matchId);
        }

        private async Task<Guid> GetParticipantLowestWeight(Guid matchId)
        {
            var match = await _matchRepository.GetMatchWithId(matchId);
            var firstParticipantCompetition = await _participantRepository.GetParticipantDto(match.CompetitorFirstId);
            var secondParticipantCompetition = await _participantRepository.GetParticipantDto(match.CompetitorSecondId);

            if (firstParticipantCompetition.CategoryWeight > secondParticipantCompetition.CategoryWeight)
            {
                return secondParticipantCompetition.Id;
            }

            return firstParticipantCompetition.Id;
        }
    }
}

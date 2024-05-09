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
                var participantsList = _participantService.ShufflingParticipants(participants).ToList();
                if (participantsList.Count() == 3)
                {
                    await AddOnlyThreeParticipantsInMatches(participantsList);
                    return;
                }
                if (participantsList.Count() % 2 != 0)
                {
                    await AddOddParticipantsNumberInMatches(participantsList);
                    return;
                }
                if (participantsList.Count() % 2 == 0)
                {
                    await AddParticipantsInMatches(participantsList);
                    return;
                }
            }

        }

        public async Task<MatchResultDto> CalculateWinnerMatch(Guid matchId)
        {
            var rounds = await _roundRepository.GetRoundsWithMatchId(matchId);
            if (rounds.Count() == 2)
            {
                var resultTwoRounds = await SetWinnerMatchTwoRounds(rounds);
                return resultTwoRounds;
            }

            var resultTreeRounds = await SetWinnerMatchTreeRounds(rounds);
            return resultTreeRounds;
        }

        private async Task<MatchResultDto> SetWinnerMatchTwoRounds(IEnumerable<RoundDto> rounds)
        {
            var roundsWithWinners = rounds.Where(elem => elem.ParticipantWinnerId != null).ToList();
            var winnerFrequency = new Dictionary<Guid, int>();

            var roundDto = new RoundDto();
            roundDto.CompetitorFirstId = rounds.First().CompetitorFirstId;
            roundDto.CompetitorSecondId = rounds.First().CompetitorSecondId;
            roundDto.MatchId = rounds.First().MatchId;
            if (!roundsWithWinners.Any())
            {
                await _roundRepository.CreateRoundForMatch(roundDto);

                return new MatchResultDto()
                {
                    StatusDescriptionMatch = "A new round was created for these equals rounds",
                    MatchStatus = false
                };
            }
            foreach (var round in roundsWithWinners)
            {
                if (!winnerFrequency.ContainsKey((Guid)round.ParticipantWinnerId))
                {
                    winnerFrequency.Add((Guid)round.ParticipantWinnerId, 0);
                }
                winnerFrequency[(Guid)round.ParticipantWinnerId]++;
            }
            var winnerMatch = winnerFrequency.MaxBy(entry => entry.Value);

            if (winnerMatch.Value == 1 && roundsWithWinners.Count() == 2)
            {
                
                await _roundRepository.CreateRoundForMatch(roundDto);

                return new MatchResultDto()
                {
                    StatusDescriptionMatch = "A new round was created for these equals rounds",
                    MatchStatus = false
                };
            }

            var participant = await _participantRepository.GetParticipant(winnerMatch.Key);
            await _matchRepository.SetWinnerInMatch(rounds.First().MatchId, winnerMatch.Key);
            return new MatchResultDto()
            {
                WinnerName = participant.Name,
                MatchStatus = true,
                StatusDescriptionMatch = "This match have a winner"
            };
        }

        private async Task<MatchResultDto> SetWinnerMatchTreeRounds(IEnumerable<RoundDto> roundsDtos)
        {
            var roundsWithWinners = roundsDtos.Where(elem => elem.ParticipantWinnerId != null).ToList();
            var winnerFrequency = new Dictionary<Guid, int>();

            if (roundsWithWinners.Count() % 2 != 0)
            {
                foreach (var roundDto in roundsWithWinners)
                {
                    if (!winnerFrequency.ContainsKey((Guid)roundDto.ParticipantWinnerId))
                    {
                        winnerFrequency.Add((Guid)roundDto.ParticipantWinnerId, 0);
                    }
                    winnerFrequency[(Guid)roundDto.ParticipantWinnerId]++;
                }
                var winnerMatch = winnerFrequency.MaxBy(entry => entry.Value);
                var participant = await _participantRepository.GetParticipant(winnerMatch.Key);
                await _matchRepository.SetWinnerInMatch(roundsDtos.First().MatchId, winnerMatch.Key);

                return new MatchResultDto()
                {
                    WinnerName = participant.Name,
                    MatchStatus = true,
                    StatusDescriptionMatch = "This match have a winner"
                };
            }

            var winnerLessWeight = await GetParticipantLowestWeight(roundsDtos.First().MatchId);
            await _matchRepository.SetWinnerInMatch(roundsDtos.First().MatchId, winnerLessWeight);
            var participantLessWeight = await _participantRepository.GetParticipant(winnerLessWeight);

            return new MatchResultDto()
            {
                WinnerName = participantLessWeight.Name,
                MatchStatus = true,
                StatusDescriptionMatch = "This match have a winner"
            };

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
        private async Task AddParticipantsInMatches(List<Participant> participants)
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
        private async Task AddOddParticipantsNumberInMatches(List<Participant> participants)
        {
            var luckyParticipant = participants.First();
            participants.Remove(luckyParticipant);
            await AddParticipantsInMatches(participants);
        }

        /// <summary>
        /// For categories with a limited number of participants
        /// (3 in this case), a round-robin format guarantees that
        /// all participants compete against each other in 3 separate matches.
        /// Eg: first participant <=> second participant
        ///     second participant <=> third participant
        ///     third participant <=> first participant
        /// </summary>
        private async Task AddOnlyThreeParticipantsInMatches(List<Participant> participants)
        {
            var participantsMatches = new List<Participant>();
            for (int participantIndex = 0; participantIndex < participants.Count() - 1; participantIndex++)
            {
                participantsMatches.Add(participants[participantIndex]);
                participantsMatches.Add(participants[participantIndex + 1]);
            }
            participantsMatches.Add(participants[0]);
            participantsMatches.Add(participants[participants.Count() - 1]);
            await AddParticipantsInMatches(participantsMatches);
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

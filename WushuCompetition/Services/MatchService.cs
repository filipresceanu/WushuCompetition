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

        public MatchService(IParticipantService participantService, IMatchRepository matchRepository, ICategoryService categoryService, IRoundService roundService)
        {
            _participantService = participantService;
            _matchRepository = matchRepository;
            _categoryService = categoryService;
            _roundService = roundService;
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

       
    }
}

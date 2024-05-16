using AutoMapper;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Data;
using WushuCompetition.Services.Interfaces;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAgeCategoryRepository _ageCategoryRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;


        public ParticipantService(IParticipantRepository participantRepository, ICategoryRepository categoryRepository, IMapper mapper, IAgeCategoryRepository ageCategoryRepository, IMatchRepository matchRepository)
        {
            _participantRepository = participantRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _ageCategoryRepository = ageCategoryRepository;
            _matchRepository = matchRepository;
        }

        public async Task<string> AddParticipantsInCompetition(Guid competitionId,
            ParticipantDto participantDto)
        {
            string response = string.Empty;
            var categories = await _categoryRepository.GetAllCategories();
            var participant = _mapper.Map<Participant>(participantDto);

            foreach (var category in categories)
            {
                if (await CheckParticipantCategory(participant, category))
                {
                    participant.Category = category;
                    await _participantRepository.AddParticipantsInCompetition(participant);
                    response = "Succesfull added in competition";
                    break;
                }
            }
            return response;
        }

        private async Task<bool> CheckParticipantCategory(Participant participant, Category category)
        {
            var ageCat = await _ageCategoryRepository.GetAgeCategoryById(category.AgeCategoryId);
            int participantAge = participant.calculateAge(participant.BirthDay);
            string categoryAge = participant.GetAgeCategory(participantAge, ageCat);
            int participantWeight = participant.CategoryWeight;
            int categoryGraterWeight = category.GraterThanWeight;
            int categoryLessWeight = category.LessThanWeight;

            if (!string.IsNullOrEmpty(categoryAge) && category.Sex == participant.Sex
                && participantWeight > categoryGraterWeight && participantWeight <= categoryLessWeight)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsDataInCompetitionId(Guid competition)
        {
            return await _participantRepository.GetParticipantsDataForCompetitionId(competition);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsRandomCategoryAndCompetition(Guid categoryId, Guid competitionId)
        {
            var participants = await _participantRepository.GetParticipantsForCategoryAndCompetition(categoryId, competitionId);
            var participantsRandom = ShufflingParticipants(participants);
            return participantsRandom;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsWinnerRandomCategoryAndCompetition(Guid categoryId, Guid competitionId)
        {
            var participants = await _participantRepository.GetParticipantsWinnersForCategoryAndCompetition(categoryId, competitionId);
            var participantsRandom = ShufflingParticipants(participants);
            return participantsRandom;
        }

        public IEnumerable<Participant> ShufflingParticipants(IEnumerable<Participant> participants)
        {
            var participantsRandom = participants.OrderBy(elem => Guid.NewGuid()).ToList();
            return participantsRandom;
        }

        public async Task<IEnumerable<ParticipantDto>> GetParticipantsInCompetitionId(Guid competitionId)
        {
            return await _participantRepository.GetParticipantsForCompetitionId(competitionId);
        }

        public async Task DeleteParticipant(Guid id)
        {
            await _participantRepository.DeleteParticipants(id);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsWhoDidNotCompete(Guid categoryId, Guid competitionId)
        {
            var matches = await _matchRepository.GetMatchesNoWinner();
            var participants =
                await _participantRepository.GetParticipantsForCategoryAndCompetition(categoryId, competitionId);

            var participantsDidnCompete = new List<Participant>();

            foreach (var match in matches)
            {
                foreach (var participant in participants)
                {
                    if (match.CompetitorFirstId!=participant.Id || match.CompetitorSecondId!=participant.Id)
                    {
                        participantsDidnCompete.Add(participant);
                    }
                }
            }

            return participantsDidnCompete;
        }

        public async Task SetMatchLooser(Guid matchId, Guid winnerId)
        {
            var match = await _matchRepository.GetMatchWithId(matchId);
            var participantLoserId= match.CompetitorFirstId;
            if (match.CompetitorSecondId != winnerId)
            {
                participantLoserId = match.CompetitorSecondId;
            }

            await _participantRepository.UpdateParticipantCompeteInNextMatch(participantLoserId, false);
        }

        public async Task<Guid> GetParticipantLowestWeight(Guid matchId)
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

using AutoMapper;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository;
using WushuCompetition.Data;

namespace WushuCompetition.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICompetitionRepository _eventRepository;
        private readonly IAgeCategoryRepository _ageCategoryRepository;
        private readonly IMapper _mapper;

        public ParticipantService(IParticipantRepository participantRepository, ICategoryRepository categoryRepository, ICompetitionRepository eventRepository, IMapper mapper, IAgeCategoryRepository ageCategoryRepository)
        {
            _participantRepository = participantRepository;
            _categoryRepository = categoryRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _ageCategoryRepository = ageCategoryRepository;
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

        public async Task<IEnumerable<Participant>> GetParticipantsDataInCompetitionId(Guid competiton)
        {
            return await _participantRepository.GetParticipantsDataForCompetitionId(competiton);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsRandomCategoyAndCompetition(Guid categoryId, Guid competitionId)
        {
            var participants = await _participantRepository.GetParticipantsForCategoryAndCompetition(categoryId, competitionId);
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
    }
}

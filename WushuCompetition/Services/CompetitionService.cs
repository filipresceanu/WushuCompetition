using AutoMapper;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Services
{
    public class CompetitionService:ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;

        public CompetitionService(ICompetitionRepository competitionRepository,IMapper mapper)
        {
            _competitionRepository = competitionRepository;
        }

        public async Task CreateCompetition(Competition competition)
        {
           await _competitionRepository.CreateCompetition(competition);
        }

        public async Task<Competition> GetCompetitionById(Guid id)
        {
            var element= await _competitionRepository.GetCompetitionId(id);
            return element;
        }

        public async Task<IEnumerable<CompetitionDto>> GetCompetitions()
        {
            var events= await _competitionRepository.GetCompetitions();
            return events;
        }
    }
}

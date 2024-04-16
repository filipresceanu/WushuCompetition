using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Repository
{
    public class CompetitionRepository:ICompetitionRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CompetitionRepository(DataContext dataContext,IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper= mapper;
        }

      

        public async Task CreateCompetition(Competition competition)
        {
            _dataContext.Competitions.Add(competition);
            await _dataContext.SaveChangesAsync();
        }

        public Task DeleteCompetition(Competition competition)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CompetitionDto>> GetCompetitions()
        {
            var events= _dataContext.Competitions.ProjectTo<CompetitionDto>
                (_mapper.ConfigurationProvider).ToListAsync();           
            return await events;
        }

        public async Task<Competition> GetCompetitionId(Guid eventId)
        {
            var competition=await _dataContext.Competitions.SingleOrDefaultAsync(element=>element.Id==eventId);
            return  competition;
        }

      
    }
}

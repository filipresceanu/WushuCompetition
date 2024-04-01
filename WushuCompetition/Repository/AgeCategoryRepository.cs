using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository
{
    public class AgeCategoryRepository : IAgeCategoryRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AgeCategoryRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task CreateAgeCategory(AgeCategoryDto ageCategoryDto)
        {
            var ageCategory =  _mapper.Map<AgeCategory>(ageCategoryDto);

            _dataContext.AgeCategories.Add(ageCategory);
            await _dataContext.SaveChangesAsync(); 
        }

        public async Task<AgeCategory> GetAgeCategoryById(Guid ageCategoryId)
        {
            var ageCategory=await _dataContext.AgeCategories.SingleOrDefaultAsync(elem=> elem.Id == ageCategoryId);
            
            return ageCategory;
        }

        public async Task<IEnumerable<AgeCategoryDto>> GetAgeCategories()
        {
            var ageCategories=await _dataContext.AgeCategories.ProjectTo<AgeCategoryDto>(_mapper.ConfigurationProvider).ToListAsync();
            return ageCategories;
        }
    }
}

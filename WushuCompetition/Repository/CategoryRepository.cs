using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CategoryRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

       

        public Task AddParticipantInCategory(Participant participant)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCategory(Category category)
        {
            _dataContext.Categories.Add(category);
            await _dataContext.SaveChangesAsync();

        }

        public async Task DeleteCategory(Guid categoryId)
        {
            var category = await GetCategory(categoryId);
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
        }

        public Task EditCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = _dataContext.Categories.ToListAsync();
            return await categories;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesDto()
        {
            var categoris = _dataContext.Categories.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();
            return await categoris;
        }

        public async Task<IEnumerable<Category>> GetCategorieForCompetitionId(Guid competitionId)
        {
            var categories = await _dataContext.Categories.Where(elem => elem.CompetitionId == competitionId).ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategory(Guid categoryId)
        {
            var category = await _dataContext.Categories.SingleOrDefaultAsync(elem => elem.Id == categoryId);
            return category;
        }

        //public async Task<Category> GetCategoryss(Guid categoryId)
        //{
        //    var participants=await _dataContext.Participants.Where(element => element.CategoryId)
        //    var category = await _dataContext.Categories.Where(element=>element.Pa)
        //    return category;
        //}
    }
}

using AutoMapper;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository;

namespace WushuCompetition.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICompetitionRepository _eventRepository;
        private readonly IAgeCategoryRepository _ageCategoryRepository;     
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, ICompetitionRepository eventRepository, IMapper mapper, IAgeCategoryRepository ageCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _ageCategoryRepository = ageCategoryRepository;
        }

        public async Task CreateCategory(CategoryDto categoryDto,
            Guid competitionId,Guid categoryAgeId)
        {
            var eventCategory=await _eventRepository.GetCompetitionId(competitionId);
            var ageCategory=await _ageCategoryRepository.GetAgeCategoryById(categoryAgeId);
            var category = _mapper.Map<Category>(categoryDto);
            category.Competition = eventCategory;
            category.AgeCategory = ageCategory;
            await _categoryRepository.CreateCategory(category);
        }

        public async Task DeleteCategory(Guid categoryId)
        {
             await _categoryRepository.DeleteCategory(categoryId);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesDto()
        {
            return await _categoryRepository.GetAllCategoriesDto();
        }

        public async Task<IEnumerable<CategoryDataDto>> GetCategoryData(Guid competitionId)
        {
            var categories=await _categoryRepository.GetCategorieForCompetitionId(competitionId);
            List<CategoryDataDto> data = new List<CategoryDataDto>();
            foreach(var category in categories)
            {
                var categoryDataDto = new CategoryDataDto();
                var ageCategoryId = category.AgeCategoryId;
                var ageCategory = await _ageCategoryRepository.GetAgeCategoryById(ageCategoryId);
                categoryDataDto.Name=ageCategory.Name;
                categoryDataDto.LessThanAge= ageCategory.LessThanAge;
                categoryDataDto.GraterThanAge= ageCategory.GraterThanAge;
                categoryDataDto.LessThanWeight=category.LessThanWeight;
                categoryDataDto.Sex=category.Sex;
                categoryDataDto.GraterThanWeight = category.GraterThanWeight;
                data.Add(categoryDataDto);
            }
            return data;
        }

        public async Task<IEnumerable<CategoryDto>>GetCategoriesForCompetitionId(Guid competitionId)
        {
            var categories = await _categoryRepository.GetCategorieForCompetitionId(competitionId);
            var categoriesDto= categories.Select(category=>_mapper.Map<CategoryDto>(category));
            return categoriesDto;
        }
    }
}

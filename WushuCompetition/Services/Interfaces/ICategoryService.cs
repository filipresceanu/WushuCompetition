using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Services.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategory(CategoryDto categoryDto, Guid competitionId, Guid categoryAgeId);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesDto();

        Task DeleteCategory(Guid categoryId);

        Task<IEnumerable<CategoryDataDto>> GetCategoryData(Guid competitionId);

        Task<IEnumerable<CategoryDto>> GetCategoriesForCompetitionId(Guid competitionId);
    }
}

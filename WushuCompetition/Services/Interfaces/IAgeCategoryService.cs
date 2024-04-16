using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Services.Interfaces
{
    public interface IAgeCategoryService
    {
        Task CreateAgeCategory(AgeCategoryDto ageCategory);

        Task<AgeCategory> GetAgeCategoryById(Guid ageCategoryId);

        Task<IEnumerable<AgeCategoryDto>> GetAgeCategories();
    }
}

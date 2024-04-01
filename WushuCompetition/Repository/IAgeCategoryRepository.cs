using System.Collections.Generic;
using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository
{
    public interface IAgeCategoryRepository
    {
        Task CreateAgeCategory(AgeCategoryDto ageCategory);

        Task<AgeCategory> GetAgeCategoryById(Guid ageCategoryId);

        Task<IEnumerable<AgeCategoryDto>> GetAgeCategories();
    }
}

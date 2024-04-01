using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository
{
    public interface ICategoryRepository
    {
        Task CreateCategory(Category category);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesDto();

        Task<IEnumerable<Category>> GetAllCategories();

        Task AddParticipantInCategory(Participant participant);

        Task<Category> GetCategory(Guid categoryId);
        Task DeleteCategory(Guid categoryId);

        Task EditCategory(Guid categoryId);

        Task<IEnumerable<Category>>GetCategorieForCompetitionId(Guid competitionId);

        //TODO edit category
        //TODO delete category
    }
}

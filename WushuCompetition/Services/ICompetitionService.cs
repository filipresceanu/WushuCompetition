using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Services
{
    public interface ICompetitionService
    {
        Task CreateCompetition(Competition competition);

        Task<IEnumerable<CompetitionDto>> GetCompetitions(); 

        Task<Competition>GetCompetitionById(Guid id);
    }
}

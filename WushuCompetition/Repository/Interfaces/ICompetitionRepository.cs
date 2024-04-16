using WushuCompetition.Dto;
using WushuCompetition.Models;

namespace WushuCompetition.Repository.Interfaces
{
    public interface ICompetitionRepository
    {
        Task CreateCompetition(Competition competition);

        Task DeleteCompetition(Competition competition);

        Task<Competition> GetCompetitionId(Guid competitionId);

        Task<IEnumerable<CompetitionDto>> GetCompetitions();

        //TODO edit event


    }
}

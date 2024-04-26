using Microsoft.AspNetCore.Identity;

namespace WushuCompetition.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<IdentityUser>> GetReferees();
    }
}

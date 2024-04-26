using Microsoft.AspNetCore.Identity;

namespace WushuCompetition.Services.Interfaces
{
    public interface IAccountService
    {
        Task DistributeReferees();
    }
}

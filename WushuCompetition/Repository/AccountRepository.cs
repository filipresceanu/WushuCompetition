using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Repository
{
    public class AccountRepository:IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityUser>> GetReferees()
        {
            var users = await _userManager.GetUsersInRoleAsync("Referee");
            return users;
        }
    }
}

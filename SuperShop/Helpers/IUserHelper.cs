using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        // Vê se existe um determinado role, se não tiver cria, se existe na tabela 
        Task CheckRoleAsync(string rolename);
        // adiciona um role a um determinado user
        Task AddUserToRoleAsync(User user, string roleName);
        // confirma se o user já tem um determinado role
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);
    }
}

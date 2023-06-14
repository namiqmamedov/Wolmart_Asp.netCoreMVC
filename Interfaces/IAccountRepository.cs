using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.AccountViewModels;

namespace Wolmart.Ecommerce.Interfaces
{
    public interface IAccountRepository
    {
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task SendEmailConfirmationEmail(AppUser appUser, string token);
        Task GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task GenerateForgotPasswordTokenAsync(AppUser appUser);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model);
    }
}

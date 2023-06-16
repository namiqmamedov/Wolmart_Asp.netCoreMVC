using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Interfaces;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.AccountViewModels;

namespace Wolmart.Ecommerce.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountRepository(RoleManager<IdentityRole> roleManager, 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            AppDbContext context,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
             return await _userManager.FindByEmailAsync(email);
        }
        public async Task SendEmailConfirmationEmail(AppUser appUser, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { appUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", appUser.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink,appUser.Id,token))
                }
            };

            await _emailService.SendEmailForEmailConfirmation(options);
        }
        public async Task SendForgotPasswordEmail(AppUser appUser, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { appUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", appUser.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink,appUser.Id,token,appUser.Email))
                }
            };

            await _emailService.SendEmailForForgotPassword(options);
        }
        public async Task SendChangePasswordNotification(AppUser appUser)
        {

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { appUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", appUser.FirstName),
                }
            };

            await _emailService.SendEmailForForgotPassword(options);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }
        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }
        public async Task GenerateEmailConfirmationTokenAsync(AppUser appUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            if (!string.IsNullOrEmpty(token))
            { 
                await SendEmailConfirmationEmail(appUser, token);
            }
        }

        public async Task GenerateForgotPasswordTokenAsync(AppUser appUser)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmail(appUser, token);
            }
        }

        public async Task GenerateLinkChangePasswordNotification(AppUser appUser)
        {
           await SendChangePasswordNotification(appUser);
        }
    }
}

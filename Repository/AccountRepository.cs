using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Interfaces;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.Repository
{
    public class AccountRepository
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

        private async Task SendEmailConfirmationEmail(AppUser appUser, string token)
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

        [HttpPost]
        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }
    }
}

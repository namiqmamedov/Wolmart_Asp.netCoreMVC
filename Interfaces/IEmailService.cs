using System.Threading.Tasks;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);
        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
        Task SendEmailForChangePasswordNotification(UserEmailOptions userEmailOptions);
    }
}
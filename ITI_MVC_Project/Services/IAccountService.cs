using Microsoft.AspNetCore.Identity;
using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterVM model);
        Task<SignInResult> LoginAsync(LoginVM model);
        Task LogoutAsync();
        Task<string?> GetUserIdByEmailAsync(string email);
        Task<string?> GenerateEmailConfirmationTokenAsync(string userId);
        Task SendEmailConfirmationAsync(string email, string firstName, string callbackUrl);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task SendPasswordResetEmailAsync(string email, string callbackUrl);
        Task<string?> GeneratePasswordResetTokenAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model);
    }
}

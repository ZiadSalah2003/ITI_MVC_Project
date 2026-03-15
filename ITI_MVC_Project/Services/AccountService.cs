using ITI_MVC_Project.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using System.Text;

namespace ITI_MVC_Project.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterVM model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, "Customer");
            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginVM model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user?.Id;
        }

        public async Task<string?> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return null;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        public async Task SendEmailConfirmationAsync(string email, string firstName, string callbackUrl)
        {
            var body = EmailBodyBuilder.GenerateEmailBody("ConfirmEmail", new Dictionary<string, string>
            {
                { "{{name}}", firstName },
                { "{{action_url}}", callbackUrl }
            });
            await _emailService.SendEmailAsync(email, "Confirm your email — ITI Shop", body);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid confirmation link." });
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            return await _userManager.ConfirmEmailAsync(user, decodedToken);
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        public async Task SendPasswordResetEmailAsync(string email, string callbackUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var firstName = user?.FirstName ?? "User";

            var body = EmailBodyBuilder.GenerateEmailBody("ResetPasswordEmail", new Dictionary<string, string>
            {
                { "{{name}}", firstName },
                { "{{action_url}}", callbackUrl }
            });

            await _emailService.SendEmailAsync(email, "Reset Your Password — ITI Shop", body);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid request." });

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            return await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
        }
    }
}

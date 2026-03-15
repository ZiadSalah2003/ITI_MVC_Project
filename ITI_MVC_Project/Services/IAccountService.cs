using Microsoft.AspNetCore.Identity;
using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterVM model);
        Task<SignInResult> LoginAsync(LoginVM model);
        Task LogoutAsync();
    }
}

using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutVM?> GetCheckoutViewAsync(string userId);
        Task<(bool Success, int OrderId, string? Error)> PlaceOrderAsync(string userId, CheckoutVM model);
    }
}

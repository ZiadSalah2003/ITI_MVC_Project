using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface IOrderService
    {
        Task<OrderListVM> GetUserOrdersAsync(string userId);
        Task<OrderDetailsVM?> GetOrderDetailsAsync(string userId, int id);
    }
}

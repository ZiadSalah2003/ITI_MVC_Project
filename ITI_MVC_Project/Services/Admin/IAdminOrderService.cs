using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services.Admin
{
    public interface IAdminOrderService
    {
        Task<AdminOrderListVM> GetAllOrdersAsync();
        Task<AdminOrderDetailsVM?> GetOrderDetailsAsync(int id);
        Task<bool> UpdateStatusAsync(int id, OrderStatus status);
    }
}

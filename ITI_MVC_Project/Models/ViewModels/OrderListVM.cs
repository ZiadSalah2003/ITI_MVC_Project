using ITI_MVC_Project.Models.Entities;

namespace ITI_MVC_Project.Models.ViewModels
{
    public class OrderListVM
    {
        public IEnumerable<OrderSummaryVM> Orders { get; set; } = new List<OrderSummaryVM>();
    }

    public class OrderSummaryVM
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
    }
}
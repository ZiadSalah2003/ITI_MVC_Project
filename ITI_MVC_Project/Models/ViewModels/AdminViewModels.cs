using ITI_MVC_Project.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ITI_MVC_Project.Models.ViewModels
{
    public class AdminProductVM
    {
        public int Id { get; set; }

        [Required] [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required] [Range(0.01, double.MaxValue, ErrorMessage = "Price must be > 0")]
        public decimal Price { get; set; }

        [Required] [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [MaxLength(500)] [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Select a category")] [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<Category>? Categories { get; set; }
    }

    public class AdminCategoryVM
    {
        public int Id { get; set; }

        [Required] [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int ProductCount { get; set; }
    }

    public class AdminOrderListVM
    {
        public IEnumerable<AdminOrderItemVM> Orders { get; set; } = new List<AdminOrderItemVM>();
    }

    public class AdminOrderItemVM
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
    }

    public class AdminOrderDetailsVM
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public IEnumerable<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
    }
}
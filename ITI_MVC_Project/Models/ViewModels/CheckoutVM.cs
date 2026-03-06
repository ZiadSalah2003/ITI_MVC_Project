using System.ComponentModel.DataAnnotations;

namespace ITI_MVC_Project.Models.ViewModels
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Shipping address is required")]
        [MaxLength(500)]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [MaxLength(200)]
        public string City { get; set; } = string.Empty;

        [MaxLength(20)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public List<CartItemVM> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
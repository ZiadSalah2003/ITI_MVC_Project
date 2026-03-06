namespace ITI_MVC_Project.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<CartItemVM> Items { get; set; } = new List<CartItemVM>();
        public decimal Subtotal => Items.Sum(i => i.Subtotal);
        public decimal Tax => Subtotal * 0.14m;
        public decimal Total => Subtotal + Tax;
        public int TotalItems => Items.Sum(i => i.Quantity);
    }

    public class CartItemVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}
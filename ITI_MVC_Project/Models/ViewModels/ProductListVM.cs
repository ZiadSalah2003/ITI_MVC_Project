using ITI_MVC_Project.Models.Entities;

namespace ITI_MVC_Project.Models.ViewModels
{
    public class ProductListVM
    {
        public IEnumerable<ProductSummaryVM> Products { get; set; } = new List<ProductSummaryVM>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public int? SelectedCategoryId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public PaginationVM Pagination { get; set; } = new PaginationVM();
    }

    public class ProductSummaryVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
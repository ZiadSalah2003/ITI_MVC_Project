using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Services;

namespace ITI_MVC_Project.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService) => _catalogService = catalogService;

        public async Task<IActionResult> Index(int? categoryId, string? search,
            string sortBy = "name", int page = 1)
        {
            var vm = await _catalogService.GetProductListAsync(categoryId, search, sortBy, page);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vm = await _catalogService.GetProductDetailsAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }
    }
}
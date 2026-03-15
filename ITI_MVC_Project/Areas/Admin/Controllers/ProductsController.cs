using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Services.Admin;

namespace ITI_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IAdminProductService _productService;
        public ProductsController(IAdminProductService productService) => _productService = productService;

        public async Task<IActionResult> Index()
        {
            var vm = await _productService.GetAllAsync();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = await _productService.GetCreateFormAsync();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminProductVM model)
        {
            if (!ModelState.IsValid)
            {
                var form = await _productService.GetCreateFormAsync();
                model.Categories = form.Categories;
                return View(model);
            }

            if (await _productService.NameExistsAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "A product with this name already exists.");
                var form = await _productService.GetCreateFormAsync();
                model.Categories = form.Categories;
                return View(model);
            }

            await _productService.CreateAsync(model);
            TempData["Success"] = "Product created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _productService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminProductVM model)
        {
            if (!ModelState.IsValid)
            {
                var existing = await _productService.GetByIdAsync(model.Id);
                model.Categories = existing?.Categories;
                return View(model);
            }

            if (await _productService.NameExistsAsync(model.Name, model.Id))
            {
                ModelState.AddModelError(nameof(model.Name), "A product with this name already exists.");
                var existing = await _productService.GetByIdAsync(model.Id);
                model.Categories = existing?.Categories;
                return View(model);
            }

            var updated = await _productService.UpdateAsync(model);
            if (!updated) return NotFound();

            TempData["Success"] = "Product updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _productService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted) return NotFound();

            TempData["Success"] = "Product deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
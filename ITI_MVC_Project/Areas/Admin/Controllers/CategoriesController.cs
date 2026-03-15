using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Services.Admin;

namespace ITI_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly IAdminCategoryService _categoryService;
        public CategoriesController(IAdminCategoryService categoryService) => _categoryService = categoryService;

        public async Task<IActionResult> Index()
        {
            var vm = await _categoryService.GetAllAsync();
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create() => View(new AdminCategoryVM());

        [HttpPost]
        public async Task<IActionResult> Create(AdminCategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _categoryService.NameExistsAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "A category with this name already exists.");
                return View(model);
            }

            await _categoryService.CreateAsync(model);
            TempData["Success"] = "Category created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _categoryService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminCategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _categoryService.NameExistsAsync(model.Name, model.Id))
            {
                ModelState.AddModelError(nameof(model.Name), "A category with this name already exists.");
                return View(model);
            }

            var updated = await _categoryService.UpdateAsync(model);
            if (!updated) return NotFound();

            TempData["Success"] = "Category updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _categoryService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                var vm = await _categoryService.GetByIdAsync(id);
                if (vm == null) return NotFound();

                ModelState.AddModelError("", "Cannot delete this category because it contains products. Please remove the products first.");
                return View(vm);
            }

            TempData["Success"] = "Category deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Repositories;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITI_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetQueryable()
                .Include(c => c.Products)
                .ToListAsync();

            var vm = categories.Select(c => new AdminCategoryVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products?.Count ?? 0
            }).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create() => View(new AdminCategoryVM());

        [HttpPost]
        public async Task<IActionResult> Create(AdminCategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Category created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();

            var vm = new AdminCategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminCategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var category = await _unitOfWork.Categories.GetByIdAsync(model.Id);
            if (category == null) return NotFound();

            category.Name = model.Name;
            category.Description = model.Description;
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Category updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(new AdminCategoryVM { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Category deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
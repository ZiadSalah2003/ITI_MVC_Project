using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Repositories;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();

            var vm = products.Select(p => new AdminProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                Categories = null
            }).ToList();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new AdminProductVM
            {
                Categories = await _unitOfWork.Categories.GetAllAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _unitOfWork.Categories.GetAllAsync();
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive,
                CategoryId = model.CategoryId
            };
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Product created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return NotFound();

            var vm = new AdminProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                Categories = await _unitOfWork.Categories.GetAllAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _unitOfWork.Categories.GetAllAsync();
                return View(model);
            }

            var product = await _unitOfWork.Products.GetByIdAsync(model.Id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.ImageUrl = model.ImageUrl;
            product.IsActive = model.IsActive;
            product.CategoryId = model.CategoryId;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Product updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var vm = new AdminProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId
            };
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return NotFound();

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Product deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
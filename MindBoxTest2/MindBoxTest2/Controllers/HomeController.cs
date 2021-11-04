using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Collections.Generic;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductDbContext _db;
        private readonly IProductManager _pm;
        public HomeController(IProductManager productManager, ProductDbContext context)
        {
            _pm = productManager;
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string product, int? category, int page = 1,
            SortState sortOrder = SortState.ProductAsc)
        {
            var model = await _pm.IndexAsync(_db, product, category, page, sortOrder);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct(int page = 1)
        {
            var categories = await _db.Categories.Include(s => s.Products).OrderBy(c => c.Name).ToListAsync();

            var selected = categories.Select(c => new SelectItem() { Id = c.Id, Name = c.Name, IsChecked = false }).ToList();

            var model = new AddProductViewModel() { Page = page, Categories = categories, Selected = selected };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _pm.AddProductAsync(_db, model);
            return RedirectToAction("Index", "Home", new { page = model.Page });
        }

        [HttpGet]
        public IActionResult AddCategory( string previousPage, int id = 1, int page = 1)
        {   
            var model = new AddCategoryViewModel() { Id = id, Page = page, PreviousPage = previousPage };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _pm.AddCategoryAsync(_db, model.Name);
            return RedirectToAction("Edit", "Home", new { id = model.Id, page = model.Page });
        }


        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id, int page = 1)
        {
            var product = await _pm.GetProductAsync(_db, id);
            var model = new DeleteProductViewModel { Product = product, Page = page};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(DeleteProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _pm.DeleteProductAsync(_db, model.Product.Id);
            return RedirectToAction("Index", "Home", new { page = model.Page });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int page = 1)
        {
            var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();

            var selected = categories.Select(c => new SelectItem() { Id = c.Id, Name = c.Name, IsChecked = false }).ToList();

            var model = new DeleteCategoryViewModel() { Page = page, Categories = categories, Selected = selected };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryViewModel model)
        {
            await _pm.DeleteCategoryAsync(_db, model);
            return RedirectToAction("Index", "Home", new { page = model.Page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1)
        {
            var product = await _pm.GetProductAsync(_db, id);

            var categories = await _db.Categories.Include(s => s.Products).OrderBy(c => c.Name).ToListAsync();

            var selected = categories.Select(c => new SelectItem() { Id = c.Id, Name = c.Name }).ToList();

            foreach (var item in selected)
            {
                if (product.Categories.Contains(categories.FirstOrDefault(c => c.Name == item.Name)))
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }

            var model = new EditViewModel() { Page = page, Product = product, Categories = categories, Selected = selected };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid) return View(model); 

            await _pm.EditAsync(_db, model);

            return RedirectToAction("Index", "Home", new { page = model.Page });
        }

    }
}
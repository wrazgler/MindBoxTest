using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using MindBoxTest2.Models;
using MindBoxTest2.Services;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public HomeController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }  

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string product, int? category, int page = 1,
            SortState sortOrder = SortState.ProductAsc)
        {
            var model = await _productService.GetProductsAsync(product, category, page, sortOrder);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct(int page = 1)
        {
            var selected = await _categoryService.GetSelectedAsync();
            var model = new AddProductViewModel() { Page = page, Selected = selected };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _productService.TryAddProductAsync(model);

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
            if (!ModelState.IsValid)
                return View(model);

            await _categoryService.TryAddCategoryAsync(model.Name);

            return RedirectToAction($"{model.PreviousPage}", "Home", new { id = model.Id, page = model.Page });

        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id, int page = 1)
        {
            var product = await _productService.GetProductAsync(id);
            var model = new DeleteProductViewModel { Product = product, Page = page};

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(DeleteProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _productService.TryDeleteProductAsync(model.Product.Id);

            return RedirectToAction("Index", "Home", new { page = model.Page });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int page = 1)
        {
            var selected = await _categoryService.GetSelectedAsync();
            var model = new DeleteCategoryViewModel() { Page = page, Selected = selected };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryViewModel model)
        {
            await _categoryService.DeleteCategoryAsync(model);

            return RedirectToAction("Index", "Home", new { page = model.Page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1)
        {
            var model = await _productService.EditGetAsync(id, page);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _productService.EditPostAsync(model);

            return RedirectToAction("Index", "Home", new { page = model.Page });
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;
 
namespace MindBoxTest2.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;
        public ProductService(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<bool> TryAddProductAsync(AddProductViewModel model)
        {
            if (model.Name == null)
                return false;

            var products = _db.Products.ToList();

            foreach (var p in products)
            {
                if (p.Name.ToLower() == model.Name.ToLower())
                    return false;
            }

            var product = new Product { Name = model.Name };

            if (model.Selected != null)
            {
                foreach (var item in model.Selected.ChekList)
                {
                    var category = await _db.Categories
                        .FirstOrDefaultAsync(c => c.Name == item.Category.Name);

                    if (item.IsChecked)
                    {
                        product.Categories.Add(category);
                    }
                }
            }
            _db.Products.Add(product);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TryDeleteProductAsync(int id)
        {
            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<EditViewModel> EditGetAsync(int id, int page = 1)
        {
            var product = await _db.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);

            var categories = await _db.Categories
                .ToListAsync();

            var selected = new CheckBoxViewModel()
            {
                ChekList = categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectItem() { Category = c })
                .ToList()
            };

            foreach (var item in selected.ChekList)
            {
                var select = categories.FirstOrDefault(c => c.Name == item.Category.Name);

                if (product.Categories.Contains(select))
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }

            var model = new EditViewModel() { Page = page, Product = product, Selected = selected };

            return model;
        }
 
        public async Task EditPostAsync(EditViewModel model)
        {
            if (model == null) return;

            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.Id == model.Product.Id);

            var categories = await _db.Categories
                .ToListAsync();

            if (product == null) return;
            product.Name = model.Product.Name;

            if (model.Selected != null)
            {
                foreach (var item in model.Selected.ChekList)
                {
                    var select = categories.FirstOrDefault(c => c.Name == item.Category.Name);

                    if (item.IsChecked && !product.Categories.Contains(select))
                    {
                        product.Categories.Add(select);
                    }
                    if (!item.IsChecked && product.Categories.Contains(select))
                    {
                        product.Categories.Remove(select);
                    }
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task<AllProductsViewModel> GetProductsAsync(string product, int category, int page, SortState sortOrder)
        {
            const int pageSize = 10;

            var products = await _db.Products
                .Include(p => p.Categories)
                .ToListAsync();

            var currentCategory = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == category);

            if (currentCategory != null && category != 0)
            {
                products = currentCategory.Products.ToList();
            }

            if (!string.IsNullOrEmpty(product))
            {
                products = products
                    .Where(p => p.Name.ToLower()
                    .Contains(product.ToLower()))
                    .ToList();
            }

            switch (sortOrder)
            {
                case SortState.ProductDesc:
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
            }

            var count = products.Count();
            var items = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new AllProductsViewModel
            {
                FilterViewModel = new FilterViewModel(_db.Products.ToList(), product, _db.Categories.ToList(), category),
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                Page = page,
                Products = items
            };

            return viewModel;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _db.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }
    }
}

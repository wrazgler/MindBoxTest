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
        public async Task AddProductAsync(ProductDbContext db, ICategoryService categoryService, AddProductViewModel model)
        {
            if (model.Name == null) return;

            var product = await db.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == model.Name.ToLower());

            if (product != null) return;
            
            product = new Product { Name = model.Name };
            
            if (model.Selected != null)
            {
                foreach (var item in model.Selected.ChekList)
                {
                    var category = await categoryService.GetCategoryAsync(db, item.Id);

                    if (item.IsChecked)
                    {
                        product.Categories.Add(category);
                    }
                }
            }
            db.Products.Add(product);

            await db.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(ProductDbContext db, int id)
        {
            var product = await GetProductAsync(db, id);

            if (product == null) return;

            db.Products.Remove(product);
            await db.SaveChangesAsync();
        }

        public async Task<Product> EditAsync(ProductDbContext db, ICategoryService categoryService, EditViewModel model)
        {
            if (model == null) return null;

            var product = await GetProductAsync(db, model.Product.Id);
            
            if (product == null) return null;

            product.Name = model.Product.Name;
            
            if(model.Selected != null)
            {
                foreach (var item in model.Selected.ChekList)
                {
                    var category = await categoryService.GetCategoryAsync(db, item.Id);

                    if (item.IsChecked && !product.Categories.Contains(category))
                    {
                        product.Categories.Add(category);
                    }
                    if (!item.IsChecked && product.Categories.Contains(category))
                    {
                        product.Categories.Remove(category);
                    }
                }
            }
            
            await db.SaveChangesAsync();

            return product;
        }

        public async Task<IndexViewModel> IndexAsync(ProductDbContext db, string product, int? category, int page = 1,
            SortState sortOrder = SortState.ProductAsc)
        {
            int pageSize = 10;

            var products = await db.Products.Include(p => p.Categories).ToListAsync();
            var currentCategory = await db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == category);

            if (currentCategory != null && category != 0)
            {
                products = currentCategory.Products.ToList();
            }

            if (!String.IsNullOrEmpty(product))
            {
                products = products.Where(p => p.Name.ToLower().Contains(product.ToLower())).ToList();
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

            IndexViewModel viewModel = new IndexViewModel
            {
                FilterViewModel = new FilterViewModel(db.Products.ToList(), product, db.Categories.ToList(), category),
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                Page = page,
                Products = items
            };
            return viewModel;
        }

        public async Task<Product> GetProductAsync(ProductDbContext db, int id)
        {
            var product = await db.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }
    }
}

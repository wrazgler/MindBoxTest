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
        private readonly string _connection;
        public ProductService(string connectionString)
        {
            _connection = connectionString;
        }

        public async Task AddProductAsync(AddProductViewModel model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                if (model.Name == null) return;

                var product = await db.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == model.Name.ToLower());

                if (product != null) return;

                product = new Product { Name = model.Name };

                if (model.Selected != null)
                {
                    foreach (var item in model.Selected.ChekList)
                    {
                        var category = await db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Name == item.Category.Name);

                        if (item.IsChecked)
                        {
                            product.Categories.Add(category);
                        }
                    }
                }
                db.Products.Add(product);

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                var product = await db.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);

                if (product == null) return;

                db.Products.Remove(product);
                await db.SaveChangesAsync();
            }
        }

        public async Task<EditViewModel> EditGetAsync(int id, int page = 1)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                var product = await db.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
                var categories = await db.Categories.Include(s => s.Products).ToListAsync();
                var selected = new CheckBoxViewModel() { ChekList = categories.Select(c => new SelectItem() { Category = c }).ToList() };

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
        }
 
        public async Task EditPostAsync(EditViewModel model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                if (model == null) return;

                var product = await db.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == model.Product.Id);
                var categories = await db.Categories.Include(s => s.Products).ToListAsync();

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

                await db.SaveChangesAsync();
            }
        }

        public async Task<IndexViewModel> IndexAsync(string product, int? category, int page, SortState sortOrder)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
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
        }

        public async Task<Product> GetProductAsync(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                var product = await db.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
                return product;
            }
        }
    }
}

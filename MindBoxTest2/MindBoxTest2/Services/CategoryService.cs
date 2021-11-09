using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly string _connection;
        public CategoryService(string connectionString) 
        {
            _connection = connectionString;
        }
        public async Task AddCategoryAsync(string name)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                if (name == null) return;

                var category = await db.Categories.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

                if (category != null) return;

                category = new Category { Name = name };
                db.Categories.Add(category);

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(DeleteCategoryViewModel model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                if (model.Selected != null)
                {
                    foreach (var item in model.Selected.ChekList)
                    {
                        var category = await db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == item.Category.Id);

                        if (item.IsChecked && category != null)
                        {
                            db.Categories.Remove(category);
                        }
                    }
                }
                await db.SaveChangesAsync();
            }   
        }

        public async Task<CheckBoxViewModel> GetSelectedAsync()
        {
            using (ApplicationDbContext db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connection).Options))
            {
                var categories = await db.Categories.Include(s => s.Products).OrderBy(c => c.Name).ToListAsync();
                var selected = new CheckBoxViewModel() { ChekList = categories.Select(c => new SelectItem() { Category = c }).ToList() };
                return selected;
            }
        }
    }
}

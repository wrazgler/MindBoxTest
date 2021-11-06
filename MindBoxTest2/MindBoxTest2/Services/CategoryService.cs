using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Services
{
    public class CategoryService : ICategoryService
    {
        public async Task AddCategoryAsync(ProductDbContext db, string name)
        {
            if (name == null) return;

            var category = await db.Categories.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            if (category != null) return;

            category = new Category { Name = name };
            db.Categories.Add(category);

            await db.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(ProductDbContext db, DeleteCategoryViewModel model)
        {
            if (model.Selected != null)
            {
                foreach (var item in model.Selected.ChekList)
                {
                    var category = await GetCategoryAsync(db, item.Id);

                    if (item.IsChecked && category != null)
                    {
                        db.Categories.Remove(category);
                    }
                }
            }
            await db.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryAsync(ProductDbContext db, int id)
        {
            var category = await db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }
    }
}

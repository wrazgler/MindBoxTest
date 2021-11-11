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
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext context) 
        {
            _db = context;
        }

        public async Task<bool> TryAddCategoryAsync(string name)
        {
            if (name == null)
                return false;

            var categories =  _db.Categories.ToList();

            foreach (var c in categories)
            {
                if (c.Name.ToLower() == name.ToLower())
                    return false;
            }

            var category = new Category { Name = name };
            _db.Categories.Add(category);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task DeleteCategoryAsync(DeleteCategoryViewModel model)
        {
            if (model.Selected != null)
            {
                foreach (var item in model.Selected.ChekList)
                {
                    var category = await _db.Categories
                        .Include(c => c.Products)
                        .FirstOrDefaultAsync(c => c.Id == item.Category.Id);

                    if (item.IsChecked && category != null)
                    {
                        _db.Categories.Remove(category);
                    }
                }
            }
            await _db.SaveChangesAsync();
        }

        public async Task<CheckBoxViewModel> GetSelectedAsync()
        {
            var categories = await _db.Categories
                .Include(s => s.Products)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var selected = new CheckBoxViewModel()
            {
                ChekList = categories
                .Select(c => new SelectItem() { Category = c })
                .ToList()
            };

            return selected;
        }
    }
}

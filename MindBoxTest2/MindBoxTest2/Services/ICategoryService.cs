using System.Threading.Tasks;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(ProductDbContext db, string name);

        Task DeleteCategoryAsync(ProductDbContext db, DeleteCategoryViewModel model);

        Task<Category> GetCategoryAsync(ProductDbContext db, int id);
    }
}

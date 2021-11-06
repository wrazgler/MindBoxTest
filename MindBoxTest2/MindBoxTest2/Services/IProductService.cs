using System.Threading.Tasks;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Services
{
    public interface IProductService
    {
        Task AddProductAsync(ProductDbContext db, ICategoryService categoryService, AddProductViewModel model);

        Task DeleteProductAsync(ProductDbContext db, int id);

        Task<Product> EditAsync(ProductDbContext db, ICategoryService categoryService, EditViewModel model);

        Task<IndexViewModel> IndexAsync(ProductDbContext db, string product, int? category, int page = 1,
            SortState sortOrder = SortState.ProductAsc);

        Task<Product> GetProductAsync(ProductDbContext db, int id);
    }
}

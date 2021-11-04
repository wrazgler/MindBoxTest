using System.Threading.Tasks;

using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Models
{
    public interface IProductManager
    {
        Task AddProductAsync(ProductDbContext db, AddProductViewModel model);

        Task AddCategoryAsync(ProductDbContext db, string name);

        Task DeleteProductAsync(ProductDbContext db, int id);

        Task DeleteCategoryAsync(ProductDbContext db, DeleteCategoryViewModel model);

        Task<Product> EditAsync(ProductDbContext db, EditViewModel model);

        Task<IndexViewModel> IndexAsync(ProductDbContext db, string product, int? category, int page = 1,
            SortState sortOrder = SortState.ProductAsc);

        Task<Product> GetProductAsync(ProductDbContext db, int id);
        Task<Category> GetCategoryAsync(ProductDbContext db, int id);
    }
}

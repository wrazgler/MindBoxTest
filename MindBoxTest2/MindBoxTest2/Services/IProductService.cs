using System.Threading.Tasks;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Services
{
    public interface IProductService
    {
        Task AddProductAsync(AddProductViewModel model);

        Task DeleteProductAsync(int id);

        Task<EditViewModel> EditGetAsync(int id, int page = 1);

        Task EditPostAsync(EditViewModel model);

        Task<IndexViewModel> IndexAsync(string product, int? category, int page, SortState sortOrder);

        Task<Product> GetProductAsync(int id);
    }
}

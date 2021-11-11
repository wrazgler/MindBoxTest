using System.Collections.Generic;
using System.Threading.Tasks;

using MindBoxTest2.Models;
using MindBoxTest2.ViewModels;

namespace MindBoxTest2.Services
{
    public interface ICategoryService
    {
        Task<bool> TryAddCategoryAsync(string name);

        Task DeleteCategoryAsync(DeleteCategoryViewModel model);

        Task<CheckBoxViewModel> GetSelectedAsync();
    }
}
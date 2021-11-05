using System.Collections.Generic;

using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class DeleteCategoryViewModel
    {
        public int Page { get; set; }
        public CheckBoxViewModel Selected { get; set; }

        public DeleteCategoryViewModel()
        {
            Selected = new CheckBoxViewModel();
        }
    }
}

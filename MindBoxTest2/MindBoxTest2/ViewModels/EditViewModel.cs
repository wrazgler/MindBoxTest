using System.Collections.Generic;

using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class EditViewModel
    {
        public int Page { get; set; }
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public IList<SelectItem> Selected { get; set; }

        public EditViewModel()
        {
            Categories = new List<Category>();
            Selected = new List<SelectItem>();
        }
    }
}

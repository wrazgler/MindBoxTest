using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class EditViewModel
    {
        public int Page { get; set; }
        public Product Product { get; set; }
        public CheckBoxViewModel Selected { get; set; }

        public EditViewModel()
        {
            Selected = new CheckBoxViewModel();
        }
    }
}

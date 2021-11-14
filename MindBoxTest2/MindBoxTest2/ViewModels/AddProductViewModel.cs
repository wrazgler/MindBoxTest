namespace MindBoxTest2.ViewModels
{
    public class AddProductViewModel
    {
        public int Page { get; set; }
        public string Name { get; set; }
        public CheckBoxViewModel Selected { get; set; }

        public AddProductViewModel()
        {
            Selected = new CheckBoxViewModel();
        }
    }
}

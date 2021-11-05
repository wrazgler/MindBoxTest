using System.Collections.Generic;

using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class CheckBoxViewModel
    {
        public List<SelectItem> ChekList { get; set; }

        public CheckBoxViewModel()
        {
            ChekList = new List<SelectItem>();
        }
    }
}

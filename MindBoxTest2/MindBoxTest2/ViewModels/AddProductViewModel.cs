﻿using System.Collections.Generic;

using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class AddProductViewModel
    {
        public int Page { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
        public IList<SelectItem> Selected { get; set; }

        public AddProductViewModel()
        {
            Categories = new List<Category>();
            Selected = new List<SelectItem>();
        }
    }
}

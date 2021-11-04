using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class FilterViewModel
    {
        public List<Product> Products { get; private set; }
        public SelectList Categories { get; private set; }
        public string SelectedProduct { get; private set; }
        public int? SelectedCategory{ get; private set; }
        public FilterViewModel(List<Product> products, string product, List<Category> categories, int? category)
        {
            Products = products;
            SelectedProduct = product;

            categories.Insert(0, new Category { Name = "Все", Id = 0 });
            Categories = new SelectList(categories, "Id", "Name", category);
            SelectedCategory = category;
        }
    }
}

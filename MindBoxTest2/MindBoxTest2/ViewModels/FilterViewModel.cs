using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class FilterViewModel
    {
        public List<Product> Products { get; }
        public SelectList Categories { get; }
        public string SelectedProduct { get; }
        public int? SelectedCategory{ get; }
        public FilterViewModel(List<Product> products, string product, List<Category> categories, int category)
        {
            Products = products;
            SelectedProduct = product;

            categories.Insert(0, new Category { Name = "Все", Id = 0 });
            Categories = new SelectList(categories, "Id", "Name", category);
            SelectedCategory = category;
        }
    }
}

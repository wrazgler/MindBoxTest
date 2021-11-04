using System.Collections.Generic;

using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class IndexViewModel
    {
        public int Page { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}

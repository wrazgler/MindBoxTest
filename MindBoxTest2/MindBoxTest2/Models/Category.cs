using System.Collections.Generic;

namespace MindBoxTest2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}

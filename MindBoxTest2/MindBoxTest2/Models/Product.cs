using System.Collections.Generic;

namespace MindBoxTest2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
    }
}

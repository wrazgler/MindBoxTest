using System.Collections.Generic;
using System.Linq;

namespace MindBoxTest2.Models
{
    public class SampleData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {

                Product apple = new Product { Name = "Яблоко" };
                Product banana = new Product { Name = "Банан" };
                Product carrot = new Product { Name = "Морковь" };
                Product lemon = new Product { Name = "Лимон" };
                Product melon = new Product { Name = "Дыня" };
                Product peach = new Product { Name = "Персик" };
                Product pear = new Product { Name = "Груша" };
                Product pepper = new Product { Name = "Перец" };
                Product pineapple = new Product { Name = "Ананас" };
                Product strawberry = new Product { Name = "Клубника" };
                Product watermelon = new Product { Name = "Арбуз " };
                context.Products.AddRange(apple, banana, carrot, lemon, melon, peach, pear, pepper, pineapple, strawberry, watermelon);

                Category big = new Category { Name = "Большой" };
                Category fruit = new Category { Name = "Фрукт" };
                Category small = new Category { Name = "Маленький" };
                Category sour = new Category { Name = "Кислый" };
                Category sweet = new Category { Name = "Сладкий" };
                Category vegetable = new Category { Name = "Овощь" };
                context.Categories.AddRange(big, fruit, small, sour, sweet, vegetable);


                big.Products.AddRange(new List<Product> { melon, pineapple, watermelon });
                fruit.Products.AddRange(new List<Product> { apple, banana, lemon, melon, peach, pear, pineapple });
                small.Products.AddRange(new List<Product> { apple, banana, carrot, lemon, peach, pear, pepper });
                sour.Products.AddRange(new List<Product> { apple, lemon, pineapple });
                sweet.Products.AddRange(new List<Product> { banana, melon, pear });
                vegetable.Products.AddRange(new List<Product> { carrot, pepper });

                context.SaveChanges();
            }
        }
    }
}

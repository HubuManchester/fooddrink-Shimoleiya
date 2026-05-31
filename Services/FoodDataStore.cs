using System.Collections.Generic;
using DailyFoodSetApp.Models;

namespace DailyFoodSetApp.Services;

public static class FoodDataStore
{
    public static readonly List<FoodItem> SeedData = new()
    {
        new() { Name = "Berry Oatmeal Bowl", Category = "Breakfast", Calories = 320, Spiciness = "Not Spicy", Description = "Healthy and high protein breakfast option." },
        new() { Name = "Spicy Chicken Wrap", Category = "Breakfast", Calories = 450, Spiciness = "Mild", Description = "Energetic wrap to start your morning." },
        new() { Name = "Steamed Veggies & Brown Rice", Category = "Lunch", Calories = 480, Spiciness = "Not Spicy", Description = "Low-fat and clean diet meal for fitness." },
        new() { Name = "Mapo Tofu Rice Bowl", Category = "Lunch", Calories = 620, Spiciness = "Medium", Description = "Classic spicy Sichuan tofu served over hot rice." },
        new() { Name = "Pan-Seared Salmon with Asparagus", Category = "Dinner", Calories = 410, Spiciness = "Not Spicy", Description = "Premium fish fillet rich in Omega-3 fatty acids." },
        new() { Name = "Szechuan Poached Beef", Category = "Dinner", Calories = 750, Spiciness = "Extra Spicy", Description = "Very authentic, rich, and highly seasoned dish." },
        new() { Name = "Sugar-Free Green Tea", Category = "Drink", Calories = 0, Spiciness = "Not Spicy", Description = "Refreshing, iced, and absolutely clean taste." },
        new() { Name = "Sparkling Fruit Juice", Category = "Drink", Calories = 110, Spiciness = "Not Spicy", Description = "Slightly sweet and bubbly natural booster." },
        new() { Name = "Avocado Toast with Egg", Category = "Breakfast", Calories = 380, Spiciness = "Not Spicy", Description = "Whole wheat toast topped with mashed avocado and a fried egg." },
        new() { Name = "Spicy Breakfast Burrito", Category = "Breakfast", Calories = 550, Spiciness = "Medium", Description = "Eggs, sausage, cheese, and jalapenos wrapped in a warm tortilla." },
        new() { Name = "Pancakes with Maple Syrup", Category = "Breakfast", Calories = 460, Spiciness = "Not Spicy", Description = "Fluffy classic pancakes served with butter and pure maple syrup." },
        new() { Name = "Turkey Bacon Muffin", Category = "Breakfast", Calories = 310, Spiciness = "Not Spicy", Description = "A lighter breakfast sandwich with turkey bacon and low-fat cheese." },
        new() { Name = "Spicy Tuna Roll", Category = "Lunch", Calories = 350, Spiciness = "Medium", Description = "Fresh tuna mixed with spicy mayo wrapped in rice and seaweed." },
        new() { Name = "Grilled Chicken Salad", Category = "Lunch", Calories = 290, Spiciness = "Not Spicy", Description = "Mixed greens with grilled chicken breast and vinaigrette." },
        new() { Name = "Beef Noodle Soup", Category = "Lunch", Calories = 580, Spiciness = "Mild", Description = "Tender beef chunks and noodles in a rich, slightly spiced broth." },
        new() { Name = "Tom Yum Goong", Category = "Lunch", Calories = 420, Spiciness = "Extra Spicy", Description = "Famous Thai hot and sour prawn soup with lemongrass." },
        new() { Name = "BLT Sandwich", Category = "Lunch", Calories = 450, Spiciness = "Not Spicy", Description = "Classic Bacon, Lettuce, and Tomato sandwich on toasted bread." },
        new() { Name = "Margherita Pizza", Category = "Dinner", Calories = 800, Spiciness = "Not Spicy", Description = "Traditional Italian pizza with fresh tomatoes, mozzarella, and basil." },
        new() { Name = "Spicy Pork Tacos", Category = "Dinner", Calories = 600, Spiciness = "Medium", Description = "Three corn tortillas filled with marinated spicy pork and cilantro." },
        new() { Name = "Vegetable Stir Fry", Category = "Dinner", Calories = 280, Spiciness = "Mild", Description = "Mixed seasonal vegetables stir-fried with a hint of chili." },
        new() { Name = "Steak and Sweet Potato Fries", Category = "Dinner", Calories = 850, Spiciness = "Not Spicy", Description = "Grilled ribeye steak served with baked sweet potato fries." },
        new() { Name = "Kung Pao Chicken", Category = "Dinner", Calories = 680, Spiciness = "Extra Spicy", Description = "Sichuan dish with chicken, peanuts, vegetables, and chili peppers." },
        new() { Name = "Pad Thai", Category = "Dinner", Calories = 720, Spiciness = "Medium", Description = "Stir-fried rice noodle dish commonly served as street food in Thailand." },
        new() { Name = "Iced Black Coffee", Category = "Drink", Calories = 5, Spiciness = "Not Spicy", Description = "Chilled black coffee, perfect for a morning kick." },
        new() { Name = "Mango Smoothie", Category = "Drink", Calories = 240, Spiciness = "Not Spicy", Description = "Blended fresh mango with a splash of milk and ice." },
        new() { Name = "Hot Chocolate", Category = "Drink", Calories = 300, Spiciness = "Not Spicy", Description = "Rich and creamy hot chocolate topped with marshmallows." },
        new() { Name = "Ginger Lemon Tea", Category = "Drink", Calories = 40, Spiciness = "Mild", Description = "Warm tea with a spicy ginger kick and refreshing lemon." },
        new() { Name = "Coconut Water", Category = "Drink", Calories = 45, Spiciness = "Not Spicy", Description = "100% natural coconut water, rich in electrolytes." },
        new() { Name = "Protein Shake", Category = "Drink", Calories = 180, Spiciness = "Not Spicy", Description = "Whey protein isolate mixed with almond milk." },
        new() { Name = "Spicy Bloody Mary", Category = "Drink", Calories = 150, Spiciness = "Extra Spicy", Description = "Tomato juice cocktail with a heavy dash of hot sauce." },
        new() { Name = "Boiled Eggs and Almonds", Category = "Breakfast", Calories = 250, Spiciness = "Not Spicy", Description = "Two hard-boiled eggs with a handful of raw almonds." },
        new() { Name = "Jalapeno Cheese Bagel", Category = "Breakfast", Calories = 400, Spiciness = "Mild", Description = "Toasted bagel infused with jalapeno slices and cream cheese." },
        new() { Name = "Quinoa Salad Bowl", Category = "Lunch", Calories = 380, Spiciness = "Not Spicy", Description = "Nutrient-dense quinoa mixed with cherry tomatoes and feta." },
        new() { Name = "Spicy Curry Udon", Category = "Lunch", Calories = 650, Spiciness = "Extra Spicy", Description = "Thick wheat noodles in a fiery Japanese curry broth." },
        new() { Name = "Lemon Herb Roast Chicken", Category = "Dinner", Calories = 550, Spiciness = "Not Spicy", Description = "Half roast chicken marinated in lemon and Mediterranean herbs." },
        new() { Name = "Buffalo Cauliflower Wings", Category = "Dinner", Calories = 320, Spiciness = "Medium", Description = "Roasted cauliflower florets tossed in spicy buffalo sauce." },
        new() { Name = "Matcha Frappe", Category = "Drink", Calories = 280, Spiciness = "Not Spicy", Description = "Ice-blended green tea beverage with whipped cream." },
        new() { Name = "Chai Tea Latte", Category = "Drink", Calories = 200, Spiciness = "Mild", Description = "Spiced black tea combined with steamed milk." },
        new() { Name = "Greek Yogurt Parfait", Category = "Breakfast", Calories = 220, Spiciness = "Not Spicy", Description = "Layered yogurt with granola and fresh strawberries." },
        new() { Name = "Spicy Chorizo Hash", Category = "Breakfast", Calories = 580, Spiciness = "Medium", Description = "Diced potatoes pan-fried with spicy Spanish chorizo." },
        new() { Name = "Caesar Salad with Prawns", Category = "Lunch", Calories = 410, Spiciness = "Not Spicy", Description = "Crisp romaine lettuce, croutons, and grilled prawns." },
        new() { Name = "Kimchi Fried Rice", Category = "Lunch", Calories = 520, Spiciness = "Medium", Description = "Korean style fried rice mixed with fermented spicy kimchi." },
        new() { Name = "Baked Cod Fish", Category = "Dinner", Calories = 340, Spiciness = "Not Spicy", Description = "Lean white fish baked with butter and dill." },
        new() { Name = "Vindaloo Curry", Category = "Dinner", Calories = 820, Spiciness = "Extra Spicy", Description = "Extremely hot Indian curry served with basmati rice." },
        new() { Name = "Fresh Orange Juice", Category = "Drink", Calories = 120, Spiciness = "Not Spicy", Description = "Freshly squeezed juice with no added sugar." }
    };
}
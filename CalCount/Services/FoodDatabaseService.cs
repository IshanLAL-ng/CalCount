using CalCount.Models;

namespace CalCount.Services
{
    /// <summary>
    /// Service for food database and search functionality
    /// </summary>
    public class FoodDatabaseService
    {
        /// <summary>
        /// Common foods database - In production, this would be loaded from API or database
        /// </summary>
        private static readonly List<Food> CommonFoods = new()
        {
            new Food
            {
                Id = 1,
                Name = "Chicken Breast",
                Category = "Protein",
                Nutrition = new FoodNutrient
                {
                    Calories = 165,
                    ProteinG = 31,
                    CarbsG = 0,
                    FatG = 3.6,
                    FiberG = 0,
                    SugarG = 0,
                    SodiumMg = 74,
                    ServingSizeG = 100
                }
            },
            new Food
            {
                Id = 2,
                Name = "Broccoli",
                Category = "Vegetable",
                Nutrition = new FoodNutrient
                {
                    Calories = 34,
                    ProteinG = 2.8,
                    CarbsG = 7,
                    FatG = 0.4,
                    FiberG = 2.4,
                    SugarG = 1.4,
                    SodiumMg = 64,
                    ServingSizeG = 100
                }
            },
            new Food
            {
                Id = 3,
                Name = "Brown Rice",
                Category = "Carbs",
                Nutrition = new FoodNutrient
                {
                    Calories = 111,
                    ProteinG = 2.6,
                    CarbsG = 23,
                    FatG = 0.9,
                    FiberG = 1.8,
                    SugarG = 0.3,
                    SodiumMg = 5,
                    ServingSizeG = 100
                }
            },
            new Food
            {
                Id = 4,
                Name = "Banana",
                Category = "Fruit",
                Nutrition = new FoodNutrient
                {
                    Calories = 89,
                    ProteinG = 1.1,
                    CarbsG = 23,
                    FatG = 0.3,
                    FiberG = 2.6,
                    SugarG = 12,
                    SodiumMg = 1,
                    ServingSizeG = 100
                }
            }
        };

        /// <summary>
        /// Search foods by name
        /// </summary>
        public static List<Food> SearchFoodByName(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Food>();

            return CommonFoods
                .Where(f => f.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Search food by barcode
        /// </summary>
        public static Food? SearchFoodByBarcode(string barcode)
        {
            return CommonFoods.FirstOrDefault(f => f.Barcode == barcode);
        }

        /// <summary>
        /// Get all foods in a category
        /// </summary>
        public static List<Food> GetFoodsByCategory(string category)
        {
            return CommonFoods
                .Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Get all favorite foods
        /// </summary>
        public static List<Food> GetFavoriteFoods()
        {
            return CommonFoods.Where(f => f.IsFavorite).ToList();
        }

        /// <summary>
        /// Add food to database
        /// </summary>
        public static void AddFood(Food food)
        {
            food.Id = CommonFoods.Any() ? CommonFoods.Max(f => f.Id) + 1 : 1;
            CommonFoods.Add(food);
        }

        /// <summary>
        /// Toggle favorite status of a food
        /// </summary>
        public static void ToggleFavorite(int foodId)
        {
            var food = CommonFoods.FirstOrDefault(f => f.Id == foodId);
            if (food != null)
            {
                food.IsFavorite = !food.IsFavorite;
            }
        }

        /// <summary>
        /// Get all available categories
        /// </summary>
        public static List<string> GetAllCategories()
        {
            return CommonFoods
                .Select(f => f.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }
    }
}

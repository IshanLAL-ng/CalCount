using CalCount.Models;

namespace CalCount.Services
{
    /// <summary>
    /// Service for recipe management
    /// </summary>
    public class RecipeService
    {
        private static List<Recipe> _recipes = new();

        /// <summary>
        /// Create a new recipe
        /// </summary>
        public static Recipe CreateRecipe(string name, string type, int userId)
        {
            var recipe = new Recipe
            {
                Id = _recipes.Any() ? _recipes.Max(r => r.Id) + 1 : 1,
                Name = name,
                Type = type,
                UserId = userId,
                CreatedDate = DateTime.Now
            };

            _recipes.Add(recipe);
            return recipe;
        }

        /// <summary>
        /// Get recipe by ID
        /// </summary>
        public static Recipe? GetRecipe(int recipeId)
        {
            return _recipes.FirstOrDefault(r => r.Id == recipeId);
        }

        /// <summary>
        /// Get all recipes for a user
        /// </summary>
        public static List<Recipe> GetUserRecipes(int userId)
        {
            return _recipes.Where(r => r.UserId == userId).ToList();
        }

        /// <summary>
        /// Get recipes by type (Savory, Sweet, etc.)
        /// </summary>
        public static List<Recipe> GetRecipesByType(string type, int userId)
        {
            return _recipes
                .Where(r => r.UserId == userId && r.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Update recipe
        /// </summary>
        public static void UpdateRecipe(Recipe recipe)
        {
            var existing = _recipes.FirstOrDefault(r => r.Id == recipe.Id);
            if (existing != null)
            {
                existing.Name = recipe.Name;
                existing.Description = recipe.Description;
                existing.Instructions = recipe.Instructions;
                existing.PrepTimeMinutes = recipe.PrepTimeMinutes;
                existing.CookTimeMinutes = recipe.CookTimeMinutes;
                existing.Servings = recipe.Servings;
                existing.Ingredients = recipe.Ingredients;
                existing.Type = recipe.Type;
            }
        }

        /// <summary>
        /// Delete recipe
        /// </summary>
        public static void DeleteRecipe(int recipeId)
        {
            var recipe = _recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe != null)
            {
                _recipes.Remove(recipe);
            }
        }

        /// <summary>
        /// Calculate recipe nutrition per serving
        /// </summary>
        public static void CalculateNutritionPerServing(Recipe recipe)
        {
            if (recipe.TotalNutrition == null || recipe.Servings <= 0)
                return;

            recipe.NutritionPerServing = new FoodNutrient
            {
                Calories = recipe.TotalNutrition.Calories / recipe.Servings,
                ProteinG = recipe.TotalNutrition.ProteinG / recipe.Servings,
                CarbsG = recipe.TotalNutrition.CarbsG / recipe.Servings,
                FatG = recipe.TotalNutrition.FatG / recipe.Servings,
                FiberG = recipe.TotalNutrition.FiberG / recipe.Servings,
                SugarG = recipe.TotalNutrition.SugarG / recipe.Servings,
                SodiumMg = recipe.TotalNutrition.SodiumMg / recipe.Servings,
                ServingSizeG = recipe.TotalNutrition.ServingSizeG / recipe.Servings
            };
        }

        /// <summary>
        /// Search recipes by name
        /// </summary>
        public static List<Recipe> SearchRecipes(string query, int userId)
        {
            return _recipes
                .Where(r => r.UserId == userId &&
                           r.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

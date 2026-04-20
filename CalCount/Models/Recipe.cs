namespace CalCount.Models
{
    /// <summary>
    /// Represents a recipe (savory or sweet)
    /// </summary>
    public class Recipe
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        /// <summary>
        /// Recipe type (Savory, Sweet, Beverage, Snack, etc.)
        /// </summary>
        public string Type { get; set; } = "Savory";
        
        /// <summary>
        /// Preparation instructions
        /// </summary>
        public string? Instructions { get; set; }
        
        /// <summary>
        /// Prep time in minutes
        /// </summary>
        public int PrepTimeMinutes { get; set; }
        
        /// <summary>
        /// Cook time in minutes
        /// </summary>
        public int CookTimeMinutes { get; set; }
        
        /// <summary>
        /// Number of servings
        /// </summary>
        public int Servings { get; set; } = 1;
        
        /// <summary>
        /// List of ingredients (comma-separated or JSON format)
        /// </summary>
        public string? Ingredients { get; set; }
        
        /// <summary>
        /// Total nutritional info for entire recipe
        /// </summary>
        public FoodNutrient? TotalNutrition { get; set; }
        
        /// <summary>
        /// Nutritional info per serving
        /// </summary>
        public FoodNutrient? NutritionPerServing { get; set; }
        
        public int UserId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

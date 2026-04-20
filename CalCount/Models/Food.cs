namespace CalCount.Models
{
    /// <summary>
    /// Represents a food item that can be logged
    /// </summary>
    public class Food
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        /// <summary>
        /// Barcode for scanning food packaging
        /// </summary>
        public string? Barcode { get; set; }
        
        /// <summary>
        /// Food category (Vegetable, Fruit, Protein, Carbs, etc.)
        /// </summary>
        public string Category { get; set; } = "Other";
        
        /// <summary>
        /// Nutritional information per serving
        /// </summary>
        public FoodNutrient? Nutrition { get; set; }
        
        /// <summary>
        /// Whether this is a favorite for quick access
        /// </summary>
        public bool IsFavorite { get; set; } = false;
        
        /// <summary>
        /// Quantity consumed
        /// </summary>
        public double Quantity { get; set; } = 1;
        
        /// <summary>
        /// Unit of measurement (grams, ounces, pieces, etc.)
        /// </summary>
        public string Unit { get; set; } = "g";
        
        /// <summary>
        /// Date and time when this food was logged
        /// </summary>
        public DateTime LoggedDateTime { get; set; } = DateTime.Now;
        
        public int UserId { get; set; }
    }
}

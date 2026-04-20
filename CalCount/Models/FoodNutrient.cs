namespace CalCount.Models
{
    /// <summary>
    /// Represents nutritional information for a food item
    /// </summary>
    public class FoodNutrient
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Calories in kcal
        /// </summary>
        public double Calories { get; set; }
        
        /// <summary>
        /// Protein in grams
        /// </summary>
        public double ProteinG { get; set; }
        
        /// <summary>
        /// Carbohydrates in grams
        /// </summary>
        public double CarbsG { get; set; }
        
        /// <summary>
        /// Fat in grams
        /// </summary>
        public double FatG { get; set; }
        
        /// <summary>
        /// Fiber in grams
        /// </summary>
        public double FiberG { get; set; }
        
        /// <summary>
        /// Sugar in grams
        /// </summary>
        public double SugarG { get; set; }
        
        /// <summary>
        /// Sodium in milligrams
        /// </summary>
        public double SodiumMg { get; set; }

        /// <summary>
        /// Serving size in grams
        /// </summary>
        public double ServingSizeG { get; set; }
    }
}

namespace CalCount.Models
{
    /// <summary>
    /// Represents a meal plan for a specific day or period
    /// </summary>
    public class MealPlan
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Start date of the meal plan
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the meal plan (null if ongoing)
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Comma-separated list of recipe IDs or meal descriptions
        /// </summary>
        public string? PlannedMeals { get; set; }
        
        /// <summary>
        /// Daily calorie target for this meal plan
        /// </summary>
        public int DailyCalorieTarget { get; set; }
        
        /// <summary>
        /// Target protein in grams per day
        /// </summary>
        public double DailyProteinTargetG { get; set; }
        
        /// <summary>
        /// Target carbs in grams per day
        /// </summary>
        public double DailyCarbsTargetG { get; set; }
        
        /// <summary>
        /// Target fat in grams per day
        /// </summary>
        public double DailyFatTargetG { get; set; }
        
        /// <summary>
        /// Whether plan is active
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        public int UserId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

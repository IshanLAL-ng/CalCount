namespace CalCount.Models
{
    /// <summary>
    /// Represents a fitness goal set by the user
    /// </summary>
    public class FitnessGoal
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        /// <summary>
        /// Goal type (WeightLoss, WeightGain, Maintain, BuildMuscle, etc.)
        /// </summary>
        public string GoalType { get; set; } = "Maintain";
        
        /// <summary>
        /// Target value (e.g., target weight in kg)
        /// </summary>
        public double TargetValue { get; set; }
        
        /// <summary>
        /// Current progress value
        /// </summary>
        public double CurrentValue { get; set; }
        
        /// <summary>
        /// Daily calorie target
        /// </summary>
        public int DailyCalorieTarget { get; set; } = 2000;
        
        /// <summary>
        /// Target protein in grams per day
        /// </summary>
        public double DailyProteinTargetG { get; set; } = 50;
        
        /// <summary>
        /// Target carbs in grams per day
        /// </summary>
        public double DailyCarbsTargetG { get; set; } = 250;
        
        /// <summary>
        /// Target fat in grams per day
        /// </summary>
        public double DailyFatTargetG { get; set; } = 65;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? TargetDate { get; set; }
        
        public int UserId { get; set; }
    }
}

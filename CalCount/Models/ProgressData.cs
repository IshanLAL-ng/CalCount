namespace CalCount.Models
{
    /// <summary>
    /// Represents daily progress tracking data
    /// </summary>
    public class ProgressData
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        /// <summary>
        /// Date of this progress record
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Total calories consumed
        /// </summary>
        public double TotalCaloriesConsumed { get; set; }
        
        /// <summary>
        /// Total calories burned from workouts
        /// </summary>
        public double TotalCaloriesBurned { get; set; }
        
        /// <summary>
        /// Net calories (consumed - burned)
        /// </summary>
        public double NetCalories => TotalCaloriesConsumed - TotalCaloriesBurned;
        
        /// <summary>
        /// Total water consumed in milliliters
        /// </summary>
        public double TotalWaterMl { get; set; }
        
        /// <summary>
        /// Total protein consumed in grams
        /// </summary>
        public double TotalProteinG { get; set; }
        
        /// <summary>
        /// Total carbs consumed in grams
        /// </summary>
        public double TotalCarbsG { get; set; }
        
        /// <summary>
        /// Total fat consumed in grams
        /// </summary>
        public double TotalFatG { get; set; }
        
        /// <summary>
        /// Current weight on this date (optional)
        /// </summary>
        public double? WeightKg { get; set; }
        
        /// <summary>
        /// Number of workouts completed
        /// </summary>
        public int WorkoutCount { get; set; }
    }
}

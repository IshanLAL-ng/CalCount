namespace CalCount.Models
{
    /// <summary>
    /// Represents a workout or exercise session
    /// </summary>
    public class Workout
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        /// <summary>
        /// Exercise category (Cardio, Strength, Flexibility, Sports, etc.)
        /// </summary>
        public string Category { get; set; } = "Cardio";
        
        /// <summary>
        /// Duration in minutes
        /// </summary>
        public int DurationMinutes { get; set; }
        
        /// <summary>
        /// Estimated calories burned
        /// </summary>
        public double CaloriesBurned { get; set; }
        
        /// <summary>
        /// Intensity level (Light, Moderate, Vigorous)
        /// </summary>
        public string Intensity { get; set; } = "Moderate";
        
        /// <summary>
        /// Date and time of the workout
        /// </summary>
        public DateTime WorkoutDateTime { get; set; } = DateTime.Now;
        
        public int UserId { get; set; }
    }
}

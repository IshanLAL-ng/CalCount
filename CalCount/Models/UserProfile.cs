namespace CalCount.Models
{
    /// <summary>
    /// Represents user profile and personal stats
    /// </summary>
    public class UserProfile
    {
        public int Id { get; set; }
        
        public string Username { get; set; } = string.Empty;
        
        public string? Email { get; set; }
        
        /// <summary>
        /// Age in years
        /// </summary>
        public int Age { get; set; }
        
        /// <summary>
        /// Height in centimeters
        /// </summary>
        public double HeightCm { get; set; }
        
        /// <summary>
        /// Weight in kilograms
        /// </summary>
        public double WeightKg { get; set; }
        
        /// <summary>
        /// Gender (Male, Female, Other)
        /// </summary>
        public string Gender { get; set; } = "Other";
        
        /// <summary>
        /// Activity level (Sedentary, LightlyActive, ModeratelyActive, VeryActive, ExtremelyActive)
        /// </summary>
        public string ActivityLevel { get; set; } = "ModeratelyActive";
        
        /// <summary>
        /// Calculated BMI
        /// </summary>
        public double BMI { get; set; }
        
        /// <summary>
        /// Calculated Basal Metabolic Rate (BMR) in calories
        /// </summary>
        public double BMR { get; set; }
        
        /// <summary>
        /// Daily calorie recommendation based on profile
        /// </summary>
        public int DailyCalorieRecommendation { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}

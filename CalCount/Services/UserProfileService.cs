using CalCount.Models;

namespace CalCount.Services
{
    /// <summary>
    /// Service for user profile management and calculations
    /// </summary>
    public class UserProfileService
    {
        /// <summary>
        /// Calculate BMI (Body Mass Index)
        /// BMI = weight(kg) / (height(m))^2
        /// </summary>
        public static double CalculateBMI(double weightKg, double heightCm)
        {
            if (heightCm <= 0) return 0;
            
            double heightM = heightCm / 100.0;
            return weightKg / (heightM * heightM);
        }

        /// <summary>
        /// Calculate Basal Metabolic Rate using Mifflin-St Jeor equation
        /// </summary>
        public static double CalculateBMR(UserProfile user)
        {
            double bmr;

            if (user.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
            {
                bmr = (10 * user.WeightKg) + (6.25 * user.HeightCm) - (5 * user.Age) + 5;
            }
            else if (user.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
            {
                bmr = (10 * user.WeightKg) + (6.25 * user.HeightCm) - (5 * user.Age) - 161;
            }
            else
            {
                // Average for other genders
                bmr = ((10 * user.WeightKg) + (6.25 * user.HeightCm) - (5 * user.Age)) - 78;
            }

            return bmr;
        }

        /// <summary>
        /// Calculate daily calorie recommendation based on BMR and activity level
        /// </summary>
        public static int CalculateDailyCalories(double bmr, string activityLevel)
        {
            double multiplier = activityLevel switch
            {
                "Sedentary" => 1.2,
                "LightlyActive" => 1.375,
                "ModeratelyActive" => 1.55,
                "VeryActive" => 1.725,
                "ExtremelyActive" => 1.9,
                _ => 1.55
            };

            return (int)(bmr * multiplier);
        }

        /// <summary>
        /// Get BMI category (Underweight, Normal, Overweight, Obese)
        /// </summary>
        public static string GetBMICategory(double bmi)
        {
            return bmi switch
            {
                < 18.5 => "Underweight",
                >= 18.5 and < 25 => "Normal Weight",
                >= 25 and < 30 => "Overweight",
                >= 30 => "Obese",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Update user profile with calculated values
        /// </summary>
        public static void UpdateProfileStats(UserProfile user)
        {
            user.BMI = CalculateBMI(user.WeightKg, user.HeightCm);
            user.BMR = CalculateBMR(user);
            user.DailyCalorieRecommendation = CalculateDailyCalories(user.BMR, user.ActivityLevel);
            user.LastUpdated = DateTime.Now;
        }
    }
}

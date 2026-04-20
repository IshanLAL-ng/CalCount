using CalCount.Models;

namespace CalCount.Services
{
    /// <summary>
    /// Service for all nutrition-related calculations
    /// </summary>
    public class NutritionService
    {
        /// <summary>
        /// Calculate daily macros needed based on user profile and goals
        /// </summary>
        public static (double protein, double carbs, double fat) CalculateMacroNeeds(
            UserProfile user,
            FitnessGoal goal)
        {
            // Using standard macro split: 30% protein, 40% carbs, 30% fat
            // Can be adjusted based on goal type
            int calorieTarget = goal.DailyCalorieTarget;
            
            double proteinCalories = calorieTarget * 0.30; // 30%
            double carbsCalories = calorieTarget * 0.40;   // 40%
            double fatCalories = calorieTarget * 0.30;     // 30%
            
            // Convert to grams (4 cal/g for protein and carbs, 9 cal/g for fat)
            double proteinG = proteinCalories / 4;
            double carbsG = carbsCalories / 4;
            double fatG = fatCalories / 9;
            
            return (proteinG, carbsG, fatG);
        }

        /// <summary>
        /// Calculate calories burned based on activity
        /// </summary>
        public static double CalculateCaloriesBurned(Workout workout, double userWeightKg)
        {
            // MET (Metabolic Equivalent) values for different exercises
            double metValue = workout.Intensity switch
            {
                "Light" => 3.0,
                "Moderate" => 5.0,
                "Vigorous" => 8.0,
                _ => 5.0
            };

            // Calories = MET × Weight(kg) × Duration(hours)
            double hours = workout.DurationMinutes / 60.0;
            return metValue * userWeightKg * hours;
        }

        /// <summary>
        /// Calculate total macros from a list of foods
        /// </summary>
        public static FoodNutrient CalculateTotalMacros(List<Food> foods)
        {
            var total = new FoodNutrient
            {
                Calories = 0,
                ProteinG = 0,
                CarbsG = 0,
                FatG = 0,
                FiberG = 0,
                SugarG = 0,
                SodiumMg = 0
            };

            foreach (var food in foods)
            {
                if (food.Nutrition == null) continue;

                double scaleFactor = food.Quantity / (food.Nutrition.ServingSizeG > 0 ? food.Nutrition.ServingSizeG : 1);

                total.Calories += food.Nutrition.Calories * scaleFactor;
                total.ProteinG += food.Nutrition.ProteinG * scaleFactor;
                total.CarbsG += food.Nutrition.CarbsG * scaleFactor;
                total.FatG += food.Nutrition.FatG * scaleFactor;
                total.FiberG += food.Nutrition.FiberG * scaleFactor;
                total.SugarG += food.Nutrition.SugarG * scaleFactor;
                total.SodiumMg += food.Nutrition.SodiumMg * scaleFactor;
            }

            return total;
        }

        /// <summary>
        /// Check if daily nutrition goals are met
        /// </summary>
        public static bool CheckGoalsMet(ProgressData progress, FitnessGoal goal)
        {
            return progress.TotalProteinG >= goal.DailyProteinTargetG &&
                   progress.TotalCarbsG >= goal.DailyCarbsTargetG * 0.95 && // 95% threshold
                   progress.TotalFatG <= goal.DailyFatTargetG * 1.05;       // 105% threshold
        }
    }
}

using CalCount.Models;

namespace CalCount.Services
{
    /// <summary>
    /// Service for analytics and progress tracking
    /// </summary>
    public class AnalyticsService
    {
        /// <summary>
        /// Aggregate daily progress data from logs
        /// </summary>
        public static ProgressData CalculateDailyProgress(
            List<Food> foodsConsumed,
            List<Workout> workoutsCompleted,
            double totalWaterMl,
            int userId,
            double userWeightKg)
        {
            var progress = new ProgressData
            {
                UserId = userId,
                Date = DateTime.Now.Date,
                TotalWaterMl = totalWaterMl,
                WorkoutCount = workoutsCompleted.Count
            };

            // Calculate food nutrition totals
            var foodNutrition = NutritionService.CalculateTotalMacros(foodsConsumed);
            progress.TotalCaloriesConsumed = foodNutrition.Calories;
            progress.TotalProteinG = foodNutrition.ProteinG;
            progress.TotalCarbsG = foodNutrition.CarbsG;
            progress.TotalFatG = foodNutrition.FatG;

            // Calculate workouts calories burned
            foreach (var workout in workoutsCompleted)
            {
                progress.TotalCaloriesBurned += NutritionService.CalculateCaloriesBurned(workout, userWeightKg);
            }

            return progress;
        }

        /// <summary>
        /// Calculate weekly averages
        /// </summary>
        public static ProgressData CalculateWeeklyAverage(List<ProgressData> weekData)
        {
            if (weekData.Count == 0)
                return new ProgressData();

            return new ProgressData
            {
                Date = DateTime.Now,
                TotalCaloriesConsumed = weekData.Average(p => p.TotalCaloriesConsumed),
                TotalCaloriesBurned = weekData.Average(p => p.TotalCaloriesBurned),
                TotalProteinG = weekData.Average(p => p.TotalProteinG),
                TotalCarbsG = weekData.Average(p => p.TotalCarbsG),
                TotalFatG = weekData.Average(p => p.TotalFatG),
                TotalWaterMl = weekData.Average(p => p.TotalWaterMl),
                WorkoutCount = (int)weekData.Average(p => p.WorkoutCount)
            };
        }

        /// <summary>
        /// Calculate progress towards goal
        /// </summary>
        public static double GetGoalProgressPercentage(double currentValue, double targetValue)
        {
            if (targetValue == 0) return 0;
            return (currentValue / targetValue) * 100;
        }

        /// <summary>
        /// Get calorie deficit/surplus
        /// </summary>
        public static double GetCalorieBalance(ProgressData progress, FitnessGoal goal)
        {
            return progress.NetCalories - goal.DailyCalorieTarget;
        }

        /// <summary>
        /// Estimate weight change based on calorie deficit/surplus
        /// 1 pound (0.45 kg) ≈ 3500 calories
        /// </summary>
        public static double EstimateWeightChangePerWeek(double dailyCalorieBalance)
        {
            return (dailyCalorieBalance * 7) / 7700; // 7700 calories per kg
        }
    }
}

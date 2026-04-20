using System.Collections.ObjectModel;
using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private int _userId = 1; // Placeholder - would come from auth
        private double _caloriesConsumed;
        private double _caloriesBurned;
        private double _netCalories;
        private double _waterConsumed;
        private int _workoutsCompleted;
        private double _dailyCalorieGoal = 2000;
        private double _proteinConsumed;
        private double _carbsConsumed;
        private double _fatConsumed;

        public double CaloriesConsumed
        {
            get => _caloriesConsumed;
            set => SetProperty(ref _caloriesConsumed, value);
        }

        public double CaloriesBurned
        {
            get => _caloriesBurned;
            set => SetProperty(ref _caloriesBurned, value);
        }

        public double NetCalories
        {
            get => _netCalories;
            set => SetProperty(ref _netCalories, value);
        }

        public double WaterConsumed
        {
            get => _waterConsumed;
            set => SetProperty(ref _waterConsumed, value);
        }

        public int WorkoutsCompleted
        {
            get => _workoutsCompleted;
            set => SetProperty(ref _workoutsCompleted, value);
        }

        public double DailyCalorieGoal
        {
            get => _dailyCalorieGoal;
            set => SetProperty(ref _dailyCalorieGoal, value);
        }

        public double ProteinConsumed
        {
            get => _proteinConsumed;
            set => SetProperty(ref _proteinConsumed, value);
        }

        public double CarbsConsumed
        {
            get => _carbsConsumed;
            set => SetProperty(ref _carbsConsumed, value);
        }

        public double FatConsumed
        {
            get => _fatConsumed;
            set => SetProperty(ref _fatConsumed, value);
        }

        public ObservableCollection<Food> RecentFoods { get; set; } = new();

        public DashboardViewModel()
        {
            Title = "Dashboard";
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // Load today's progress
            var foods = LocalStorageService.LoadFoodLogs();
            var workouts = LocalStorageService.LoadWorkoutLogs();
            var waterLogs = LocalStorageService.LoadWaterLogs();

            var userProfile = LocalStorageService.LoadUserProfile();
            if (userProfile != null)
            {
                DailyCalorieGoal = userProfile.DailyCalorieRecommendation;
            }

            // Filter for today
            var today = DateTime.Now.Date;
            var todaysFoods = foods.Where(f => f.LoggedDateTime.Date == today).ToList();
            var todaysWorkouts = workouts.Where(w => w.WorkoutDateTime.Date == today).ToList();
            var todaysWater = waterLogs.Where(w => w.LoggedDateTime.Date == today).ToList();

            // Calculate totals
            if (todaysFoods.Any())
            {
                var nutrition = NutritionService.CalculateTotalMacros(todaysFoods);
                CaloriesConsumed = nutrition.Calories;
                ProteinConsumed = nutrition.ProteinG;
                CarbsConsumed = nutrition.CarbsG;
                FatConsumed = nutrition.FatG;
            }

            if (todaysWorkouts.Any() && userProfile != null)
            {
                CaloriesBurned = todaysWorkouts.Sum(w => NutritionService.CalculateCaloriesBurned(w, userProfile.WeightKg));
                WorkoutsCompleted = todaysWorkouts.Count;
            }

            WaterConsumed = todaysWater.Sum(w => w.AmountMl);
            NetCalories = CaloriesConsumed - CaloriesBurned;

            // Load recent foods for quick access
            RecentFoods.Clear();
            var recentFoods = todaysFoods.OrderByDescending(f => f.LoggedDateTime).Take(5);
            foreach (var food in recentFoods)
            {
                RecentFoods.Add(food);
            }
        }

        public void RefreshData()
        {
            LoadDashboardData();
        }

        public double GetCalorieProgress()
        {
            return DailyCalorieGoal > 0 ? (CaloriesConsumed / DailyCalorieGoal) * 100 : 0;
        }
    }
}

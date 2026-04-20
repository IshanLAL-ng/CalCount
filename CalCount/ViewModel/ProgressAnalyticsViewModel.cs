using System.Collections.ObjectModel;
using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class ProgressAnalyticsViewModel : BaseViewModel
    {
        private int _userId = 1;
        private int _selectedDays = 7; // Default to weekly view
        private double _averageDailyCalories;
        private double _averageDailyProtein;
        private double _averageDailyCarbs;
        private double _averageDailyFat;
        private double _totalCaloriesBurned;
        private double _weightChange;
        private int _workoutCount;

        public int SelectedDays
        {
            get => _selectedDays;
            set
            {
                SetProperty(ref _selectedDays, value);
                LoadAnalytics();
            }
        }

        public double AverageDailyCalories
        {
            get => _averageDailyCalories;
            set => SetProperty(ref _averageDailyCalories, value);
        }

        public double AverageDailyProtein
        {
            get => _averageDailyProtein;
            set => SetProperty(ref _averageDailyProtein, value);
        }

        public double AverageDailyCarbs
        {
            get => _averageDailyCarbs;
            set => SetProperty(ref _averageDailyCarbs, value);
        }

        public double AverageDailyFat
        {
            get => _averageDailyFat;
            set => SetProperty(ref _averageDailyFat, value);
        }

        public double TotalCaloriesBurned
        {
            get => _totalCaloriesBurned;
            set => SetProperty(ref _totalCaloriesBurned, value);
        }

        public double WeightChange
        {
            get => _weightChange;
            set => SetProperty(ref _weightChange, value);
        }

        public int WorkoutCount
        {
            get => _workoutCount;
            set => SetProperty(ref _workoutCount, value);
        }

        public ObservableCollection<ProgressData> ProgressHistory { get; set; } = new();

        public ProgressAnalyticsViewModel()
        {
            Title = "Progress Analytics";
            LoadAnalytics();
        }

        private void LoadAnalytics()
        {
            ProgressHistory.Clear();

            var foods = LocalStorageService.LoadFoodLogs();
            var workouts = LocalStorageService.LoadWorkoutLogs();
            var waterLogs = LocalStorageService.LoadWaterLogs();
            var userProfile = LocalStorageService.LoadUserProfile();

            var startDate = DateTime.Now.AddDays(-SelectedDays).Date;
            var endDate = DateTime.Now.Date;

            // Group by day and calculate progress
            var dailyProgress = new Dictionary<DateTime, (List<Food> foods, List<Workout> workouts, List<WaterLog> water)>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dayFoods = foods.Where(f => f.LoggedDateTime.Date == date).ToList();
                var dayWorkouts = workouts.Where(w => w.WorkoutDateTime.Date == date).ToList();
                var dayWater = waterLogs.Where(w => w.LoggedDateTime.Date == date).ToList();

                if (dayFoods.Any() || dayWorkouts.Any() || dayWater.Any())
                {
                    dailyProgress[date] = (dayFoods, dayWorkouts, dayWater);
                }
            }

            // Calculate aggregates
            var progressDataList = new List<ProgressData>();
            foreach (var (date, (dayFoods, dayWorkouts, dayWater)) in dailyProgress)
            {
                if (userProfile != null)
                {
                    var progress = AnalyticsService.CalculateDailyProgress(dayFoods, dayWorkouts, dayWater.Sum(w => w.AmountMl), _userId, userProfile.WeightKg);
                    progress.Date = date;
                    progressDataList.Add(progress);
                    ProgressHistory.Add(progress);
                }
            }

            // Calculate averages
            if (progressDataList.Any())
            {
                var weekAverage = AnalyticsService.CalculateWeeklyAverage(progressDataList);
                AverageDailyCalories = weekAverage.TotalCaloriesConsumed;
                AverageDailyProtein = weekAverage.TotalProteinG;
                AverageDailyCarbs = weekAverage.TotalCarbsG;
                AverageDailyFat = weekAverage.TotalFatG;
                TotalCaloriesBurned = progressDataList.Sum(p => p.TotalCaloriesBurned);
                WorkoutCount = progressDataList.Sum(p => p.WorkoutCount);

                // Estimate weight change
                var avgDailyBalance = progressDataList.Average(p => p.NetCalories);
                WeightChange = AnalyticsService.EstimateWeightChangePerWeek(avgDailyBalance);
            }
        }

        public void ExportReport()
        {
            // TODO: Implement PDF export
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert("Export", "Report export feature coming soon!", "OK")!;
            });
        }
    }
}

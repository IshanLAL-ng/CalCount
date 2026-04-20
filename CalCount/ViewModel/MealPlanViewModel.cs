using System.Collections.ObjectModel;
using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class MealPlanViewModel : BaseViewModel
    {
        private int _userId = 1;
        private string _mealPlanName = string.Empty;
        private int _dailyCalorieTarget = 2000;
        private double _dailyProteinTarget = 150;
        private double _dailyCarbsTarget = 200;
        private double _dailyFatTarget = 65;
        private DateTime _startDate = DateTime.Now;
        private DateTime? _endDate;
        private bool _isActive = true;
        private MealPlan? _selectedMealPlan;

        public string MealPlanName
        {
            get => _mealPlanName;
            set => SetProperty(ref _mealPlanName, value);
        }

        public int DailyCalorieTarget
        {
            get => _dailyCalorieTarget;
            set => SetProperty(ref _dailyCalorieTarget, value);
        }

        public double DailyProteinTarget
        {
            get => _dailyProteinTarget;
            set => SetProperty(ref _dailyProteinTarget, value);
        }

        public double DailyCarbsTarget
        {
            get => _dailyCarbsTarget;
            set => SetProperty(ref _dailyCarbsTarget, value);
        }

        public double DailyFatTarget
        {
            get => _dailyFatTarget;
            set => SetProperty(ref _dailyFatTarget, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public MealPlan? SelectedMealPlan
        {
            get => _selectedMealPlan;
            set => SetProperty(ref _selectedMealPlan, value);
        }

        public ObservableCollection<MealPlan> MealPlans { get; set; } = new();
        public ObservableCollection<string> PredefinedPlans { get; set; } = new()
        {
            "Keto Diet",
            "High Protein",
            "Balanced Diet",
            "Low Carb",
            "Vegetarian",
            "Vegan"
        };

        public MealPlanViewModel()
        {
            Title = "Meal Plans";
            LoadMealPlans();
        }

        private void LoadMealPlans()
        {
            MealPlans.Clear();
            // TODO: Load from persistent storage
            // For now, this is placeholder implementation
        }

        public void CreateMealPlan()
        {
            if (string.IsNullOrWhiteSpace(MealPlanName))
                return;

            var mealPlan = new MealPlan
            {
                Id = MealPlans.Any() ? MealPlans.Max(m => m.Id) + 1 : 1,
                Name = MealPlanName,
                StartDate = StartDate,
                EndDate = EndDate,
                DailyCalorieTarget = DailyCalorieTarget,
                DailyProteinTargetG = DailyProteinTarget,
                DailyCarbsTargetG = DailyCarbsTarget,
                DailyFatTargetG = DailyFatTarget,
                IsActive = IsActive,
                UserId = _userId,
                CreatedDate = DateTime.Now
            };

            MealPlans.Add(mealPlan);
            ResetForm();
        }

        public void ApplyPredefinedPlan(string planName)
        {
            MealPlanName = planName;

            // Set macros based on plan type
            (int calories, double protein, double carbs, double fat) = planName switch
            {
                "Keto Diet" => (2000, 100, 50, 155),
                "High Protein" => (2000, 200, 150, 65),
                "Balanced Diet" => (2000, 150, 200, 65),
                "Low Carb" => (1800, 150, 100, 80),
                "Vegetarian" => (1900, 120, 220, 60),
                "Vegan" => (1900, 110, 250, 55),
                _ => (2000, 150, 200, 65)
            };

            DailyCalorieTarget = calories;
            DailyProteinTarget = protein;
            DailyCarbsTarget = carbs;
            DailyFatTarget = fat;
        }

        public void SelectMealPlan(MealPlan mealPlan)
        {
            SelectedMealPlan = mealPlan;
            MealPlanName = mealPlan.Name;
            DailyCalorieTarget = mealPlan.DailyCalorieTarget;
            DailyProteinTarget = mealPlan.DailyProteinTargetG;
            DailyCarbsTarget = mealPlan.DailyCarbsTargetG;
            DailyFatTarget = mealPlan.DailyFatTargetG;
            StartDate = mealPlan.StartDate;
            EndDate = mealPlan.EndDate;
            IsActive = mealPlan.IsActive;
        }

        public void UpdateMealPlan()
        {
            if (SelectedMealPlan == null)
                return;

            SelectedMealPlan.Name = MealPlanName;
            SelectedMealPlan.DailyCalorieTarget = DailyCalorieTarget;
            SelectedMealPlan.DailyProteinTargetG = DailyProteinTarget;
            SelectedMealPlan.DailyCarbsTargetG = DailyCarbsTarget;
            SelectedMealPlan.DailyFatTargetG = DailyFatTarget;
            SelectedMealPlan.StartDate = StartDate;
            SelectedMealPlan.EndDate = EndDate;
            SelectedMealPlan.IsActive = IsActive;

            ResetForm();
        }

        public void DeleteMealPlan(MealPlan mealPlan)
        {
            MealPlans.Remove(mealPlan);
        }

        private void ResetForm()
        {
            MealPlanName = string.Empty;
            DailyCalorieTarget = 2000;
            DailyProteinTarget = 150;
            DailyCarbsTarget = 200;
            DailyFatTarget = 65;
            StartDate = DateTime.Now;
            EndDate = null;
            IsActive = true;
            SelectedMealPlan = null;
        }
    }
}

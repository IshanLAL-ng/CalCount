using System.Collections.ObjectModel;
using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class FitnessLoggingViewModel : BaseViewModel
    {
        private int _userId = 1;
        private string _workoutName = string.Empty;
        private int _durationMinutes = 30;
        private string _selectedIntensity = "Moderate";
        private string _selectedCategory = "Cardio";
        private string _notes = string.Empty;
        private double _estimatedCalories;

        public string WorkoutName
        {
            get => _workoutName;
            set => SetProperty(ref _workoutName, value);
        }

        public int DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                SetProperty(ref _durationMinutes, value);
                UpdateEstimatedCalories();
            }
        }

        public string SelectedIntensity
        {
            get => _selectedIntensity;
            set
            {
                SetProperty(ref _selectedIntensity, value);
                UpdateEstimatedCalories();
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public double EstimatedCalories
        {
            get => _estimatedCalories;
            set => SetProperty(ref _estimatedCalories, value);
        }

        public ObservableCollection<string> Intensities { get; set; } = new() { "Light", "Moderate", "Vigorous" };
        public ObservableCollection<string> Categories { get; set; } = new()
        {
            "Cardio", "Strength", "Flexibility", "Sports", "Yoga", "Other"
        };

        public ObservableCollection<Workout> RecentWorkouts { get; set; } = new();

        public FitnessLoggingViewModel()
        {
            Title = "Log Fitness";
            LoadRecentWorkouts();
        }

        private void UpdateEstimatedCalories()
        {
            var userProfile = LocalStorageService.LoadUserProfile();
            if (userProfile != null)
            {
                var workout = new Workout
                {
                    DurationMinutes = DurationMinutes,
                    Intensity = SelectedIntensity
                };
                EstimatedCalories = NutritionService.CalculateCaloriesBurned(workout, userProfile.WeightKg);
            }
        }

        private void LoadRecentWorkouts()
        {
            RecentWorkouts.Clear();
            var workouts = LocalStorageService.LoadWorkoutLogs();
            var recent = workouts.OrderByDescending(w => w.WorkoutDateTime).Take(5);
            foreach (var workout in recent)
            {
                RecentWorkouts.Add(workout);
            }
        }

        public void LogWorkout()
        {
            if (string.IsNullOrWhiteSpace(WorkoutName))
                return;

            var workout = new Workout
            {
                Name = WorkoutName,
                Category = SelectedCategory,
                DurationMinutes = DurationMinutes,
                Intensity = SelectedIntensity,
                Description = Notes,
                WorkoutDateTime = DateTime.Now,
                UserId = _userId
            };

            var userProfile = LocalStorageService.LoadUserProfile();
            if (userProfile != null)
            {
                workout.CaloriesBurned = NutritionService.CalculateCaloriesBurned(workout, userProfile.WeightKg);
            }

            var logs = LocalStorageService.LoadWorkoutLogs();
            logs.Add(workout);
            LocalStorageService.SaveWorkoutLogs(logs);

            LoadRecentWorkouts();
            ResetForm();
        }

        public void DeleteWorkout(Workout workout)
        {
            var logs = LocalStorageService.LoadWorkoutLogs();
            var toRemove = logs.FirstOrDefault(w => w.Id == workout.Id);
            if (toRemove != null)
            {
                logs.Remove(toRemove);
                LocalStorageService.SaveWorkoutLogs(logs);
                LoadRecentWorkouts();
            }
        }

        private void ResetForm()
        {
            WorkoutName = string.Empty;
            DurationMinutes = 30;
            SelectedIntensity = "Moderate";
            SelectedCategory = "Cardio";
            Notes = string.Empty;
            EstimatedCalories = 0;
        }
    }
}

using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class UserProfileViewModel : BaseViewModel
    {
        private string _username = string.Empty;
        private string _email = string.Empty;
        private int _age = 25;
        private double _heightCm = 170;
        private double _weightKg = 70;
        private string _selectedGender = "Other";
        private string _selectedActivityLevel = "ModeratelyActive";
        private double _bmi;
        private double _bmr;
        private int _dailyCalorieRecommendation;
        private string _bmiCategory = string.Empty;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public int Age
        {
            get => _age;
            set
            {
                SetProperty(ref _age, value);
                CalculateStats();
            }
        }

        public double HeightCm
        {
            get => _heightCm;
            set
            {
                SetProperty(ref _heightCm, value);
                CalculateStats();
            }
        }

        public double WeightKg
        {
            get => _weightKg;
            set
            {
                SetProperty(ref _weightKg, value);
                CalculateStats();
            }
        }

        public string SelectedGender
        {
            get => _selectedGender;
            set
            {
                SetProperty(ref _selectedGender, value);
                CalculateStats();
            }
        }

        public string SelectedActivityLevel
        {
            get => _selectedActivityLevel;
            set
            {
                SetProperty(ref _selectedActivityLevel, value);
                CalculateStats();
            }
        }

        public double BMI
        {
            get => _bmi;
            set => SetProperty(ref _bmi, value);
        }

        public double BMR
        {
            get => _bmr;
            set => SetProperty(ref _bmr, value);
        }

        public int DailyCalorieRecommendation
        {
            get => _dailyCalorieRecommendation;
            set => SetProperty(ref _dailyCalorieRecommendation, value);
        }

        public string BMICategory
        {
            get => _bmiCategory;
            set => SetProperty(ref _bmiCategory, value);
        }

        public List<string> Genders { get; set; } = new() { "Male", "Female", "Other" };
        public List<string> ActivityLevels { get; set; } = new()
        {
            "Sedentary",
            "LightlyActive",
            "ModeratelyActive",
            "VeryActive",
            "ExtremelyActive"
        };

        public UserProfileViewModel()
        {
            Title = "User Profile";
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            var profile = LocalStorageService.LoadUserProfile();
            if (profile != null)
            {
                Username = profile.Username;
                Email = profile.Email ?? string.Empty;
                Age = profile.Age;
                HeightCm = profile.HeightCm;
                WeightKg = profile.WeightKg;
                SelectedGender = profile.Gender;
                SelectedActivityLevel = profile.ActivityLevel;
                BMI = profile.BMI;
                BMR = profile.BMR;
                DailyCalorieRecommendation = profile.DailyCalorieRecommendation;
                BMICategory = UserProfileService.GetBMICategory(BMI);
            }
            else
            {
                // Create default profile
                CalculateStats();
            }
        }

        private void CalculateStats()
        {
            BMI = UserProfileService.CalculateBMI(WeightKg, HeightCm);
            BMICategory = UserProfileService.GetBMICategory(BMI);

            var tempProfile = new UserProfile
            {
                Age = Age,
                WeightKg = WeightKg,
                HeightCm = HeightCm,
                Gender = SelectedGender,
                ActivityLevel = SelectedActivityLevel
            };

            BMR = UserProfileService.CalculateBMR(tempProfile);
            DailyCalorieRecommendation = UserProfileService.CalculateDailyCalories(BMR, SelectedActivityLevel);
        }

        public void SaveProfile()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current?.MainPage?.DisplayAlert("Error", "Please enter a username", "OK")!;
                });
                return;
            }

            var profile = new UserProfile
            {
                Username = Username,
                Email = Email,
                Age = Age,
                HeightCm = HeightCm,
                WeightKg = WeightKg,
                Gender = SelectedGender,
                ActivityLevel = SelectedActivityLevel
            };

            UserProfileService.UpdateProfileStats(profile);
            LocalStorageService.SaveUserProfile(profile);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert("Success", "Profile saved!", "OK")!;
            });
        }

        public string GetBMIAdvice()
        {
            return BMICategory switch
            {
                "Underweight" => "Consider consulting a nutritionist about healthy weight gain.",
                "Normal Weight" => "Great! Maintain your current healthy weight.",
                "Overweight" => "Consider increasing physical activity and monitoring calories.",
                "Obese" => "Consult with a healthcare provider for personalized guidance.",
                _ => "Keep track of your BMI regularly."
            };
        }

        public string GetCalorieAdvice()
        {
            return $"Based on your profile, aim for approximately {DailyCalorieRecommendation} calories per day.";
        }
    }
}

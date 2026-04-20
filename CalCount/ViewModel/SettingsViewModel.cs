using System.Collections.ObjectModel;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private int _userId = 1;
        private bool _darkModeEnabled = false;
        private bool _notificationsEnabled = true;
        private string _selectedTheme = "Light";
        private bool _reminderBreakfast = true;
        private bool _reminderLunch = true;
        private bool _reminderDinner = true;
        private bool _reminderWater = true;
        private string _breakfastTime = "08:00";
        private string _lunchTime = "12:00";
        private string _dinnerTime = "18:00";
        private string _waterReminderTime = "09:00";

        public bool DarkModeEnabled
        {
            get => _darkModeEnabled;
            set
            {
                SetProperty(ref _darkModeEnabled, value);
                ApplyTheme();
            }
        }

        public bool NotificationsEnabled
        {
            get => _notificationsEnabled;
            set => SetProperty(ref _notificationsEnabled, value);
        }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                SetProperty(ref _selectedTheme, value);
                ApplyTheme();
            }
        }

        public bool ReminderBreakfast
        {
            get => _reminderBreakfast;
            set => SetProperty(ref _reminderBreakfast, value);
        }

        public bool ReminderLunch
        {
            get => _reminderLunch;
            set => SetProperty(ref _reminderLunch, value);
        }

        public bool ReminderDinner
        {
            get => _reminderDinner;
            set => SetProperty(ref _reminderDinner, value);
        }

        public bool ReminderWater
        {
            get => _reminderWater;
            set => SetProperty(ref _reminderWater, value);
        }

        public string BreakfastTime
        {
            get => _breakfastTime;
            set => SetProperty(ref _breakfastTime, value);
        }

        public string LunchTime
        {
            get => _lunchTime;
            set => SetProperty(ref _lunchTime, value);
        }

        public string DinnerTime
        {
            get => _dinnerTime;
            set => SetProperty(ref _dinnerTime, value);
        }

        public string WaterReminderTime
        {
            get => _waterReminderTime;
            set => SetProperty(ref _waterReminderTime, value);
        }

        public ObservableCollection<string> Themes { get; set; } = new() { "Light", "Dark", "System" };

        public SettingsViewModel()
        {
            Title = "Settings";
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Load saved settings from storage
            var reminders = NotificationService.GetUserReminders(_userId);
            ReminderBreakfast = reminders.Any(r => r.ReminderType == "LogFood" && r.Title.Contains("Breakfast"));
            ReminderLunch = reminders.Any(r => r.ReminderType == "LogFood" && r.Title.Contains("Lunch"));
            ReminderDinner = reminders.Any(r => r.ReminderType == "LogFood" && r.Title.Contains("Dinner"));
            ReminderWater = reminders.Any(r => r.ReminderType == "DrinkWater");
        }

        private void ApplyTheme()
        {
            // Apply theme to the app
            string theme = SelectedTheme switch
            {
                "Dark" => "DarkTheme",
                "Light" => "LightTheme",
                _ => "SystemTheme"
            };

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current?.Resources.MergedDictionaries != null)
                {
                    // TODO: Implement theme switching
                }
            });
        }

        public void SaveSettings()
        {
            // Save reminder settings
            if (ReminderBreakfast)
            {
                if (TimeSpan.TryParse(BreakfastTime, out var time))
                {
                    NotificationService.CreateReminder(
                        "Breakfast Reminder",
                        "Time to log your breakfast!",
                        "LogFood",
                        time,
                        _userId);
                }
            }

            if (ReminderLunch)
            {
                if (TimeSpan.TryParse(LunchTime, out var time))
                {
                    NotificationService.CreateReminder(
                        "Lunch Reminder",
                        "Time to log your lunch!",
                        "LogFood",
                        time,
                        _userId);
                }
            }

            if (ReminderDinner)
            {
                if (TimeSpan.TryParse(DinnerTime, out var time))
                {
                    NotificationService.CreateReminder(
                        "Dinner Reminder",
                        "Time to log your dinner!",
                        "LogFood",
                        time,
                        _userId);
                }
            }

            if (ReminderWater)
            {
                if (TimeSpan.TryParse(WaterReminderTime, out var time))
                {
                    NotificationService.CreateReminder(
                        "Hydration Reminder",
                        "Time to drink some water!",
                        "DrinkWater",
                        time,
                        _userId);
                }
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert("Success", "Settings saved!", "OK")!;
            });
        }

        public void ResetToDefaults()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                bool confirmed = await Application.Current?.MainPage?.DisplayAlert("Reset", "Reset all settings to default?", "Yes", "No")!;
                if (confirmed)
                {
                    DarkModeEnabled = false;
                    SelectedTheme = "Light";
                    NotificationsEnabled = true;
                    ReminderBreakfast = true;
                    ReminderLunch = true;
                    ReminderDinner = true;
                    ReminderWater = true;
                    BreakfastTime = "08:00";
                    LunchTime = "12:00";
                    DinnerTime = "18:00";
                    WaterReminderTime = "09:00";
                }
            });
        }
    }
}

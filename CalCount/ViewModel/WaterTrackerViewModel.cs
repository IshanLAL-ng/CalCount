using System.Collections.ObjectModel;
using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class WaterTrackerViewModel : BaseViewModel
    {
        private int _userId = 1;
        private double _totalWaterToday;
        private double _waterGoalMl = 2000;
        private double _recentWaterAmount = 250; // 1 cup
        private string _selectedWaterUnit = "ml";

        public double TotalWaterToday
        {
            get => _totalWaterToday;
            set => SetProperty(ref _totalWaterToday, value);
        }

        public double WaterGoalMl
        {
            get => _waterGoalMl;
            set => SetProperty(ref _waterGoalMl, value);
        }

        public double RecentWaterAmount
        {
            get => _recentWaterAmount;
            set => SetProperty(ref _recentWaterAmount, value);
        }

        public string SelectedWaterUnit
        {
            get => _selectedWaterUnit;
            set => SetProperty(ref _selectedWaterUnit, value);
        }

        public ObservableCollection<string> WaterUnits { get; set; } = new() { "ml", "oz", "cup" };
        public ObservableCollection<WaterLog> TodaysWaterLogs { get; set; } = new();

        public WaterTrackerViewModel()
        {
            Title = "Water Tracker";
            LoadTodayWater();
        }

        private void LoadTodayWater()
        {
            TodaysWaterLogs.Clear();
            var waterLogs = LocalStorageService.LoadWaterLogs();
            var today = DateTime.Now.Date;
            var todaysLogs = waterLogs.Where(w => w.LoggedDateTime.Date == today).ToList();

            TotalWaterToday = todaysLogs.Sum(w => w.AmountMl);

            foreach (var log in todaysLogs.OrderByDescending(w => w.LoggedDateTime))
            {
                TodaysWaterLogs.Add(log);
            }
        }

        public void LogWater(double amount)
        {
            // Convert to ml if needed
            double amountMl = SelectedWaterUnit switch
            {
                "oz" => amount * 29.5735, // 1 oz = 29.5735 ml
                "cup" => amount * 236.588, // 1 cup = 236.588 ml
                _ => amount // already in ml
            };

            var waterLog = new WaterLog
            {
                AmountMl = amountMl,
                LoggedDateTime = DateTime.Now,
                UserId = _userId
            };

            var logs = LocalStorageService.LoadWaterLogs();
            logs.Add(waterLog);
            LocalStorageService.SaveWaterLogs(logs);

            LoadTodayWater();
        }

        public void LogWaterQuick(int option)
        {
            // Quick log options
            double amount = option switch
            {
                1 => 250,  // 1 cup
                2 => 500,  // 2 cups
                3 => 1000, // 4 cups
                _ => 250
            };

            LogWater(amount);
        }

        public void DeleteWaterLog(WaterLog log)
        {
            var logs = LocalStorageService.LoadWaterLogs();
            var toRemove = logs.FirstOrDefault(w => w.Id == log.Id);
            if (toRemove != null)
            {
                logs.Remove(toRemove);
                LocalStorageService.SaveWaterLogs(logs);
                LoadTodayWater();
            }
        }

        public double GetWaterProgress()
        {
            return WaterGoalMl > 0 ? (TotalWaterToday / WaterGoalMl) * 100 : 0;
        }

        public string GetWaterStatus()
        {
            var progress = GetWaterProgress();
            if (progress >= 100)
                return "Goal Reached! 🎉";
            else if (progress >= 50)
                return "Keep it up! 💪";
            else
                return "Let's hydrate! 💧";
        }
    }
}

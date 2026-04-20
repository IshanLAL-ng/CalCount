namespace CalCount.Services
{
    /// <summary>
    /// Service for managing notifications and reminders
    /// </summary>
    public class NotificationService
    {
        private static List<Reminder> _reminders = new();

        public class Reminder
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string ReminderType { get; set; } = string.Empty; // LogFood, LogWorkout, DrinkWater, etc.
            public TimeSpan Time { get; set; }
            public bool IsEnabled { get; set; } = true;
            public List<int> DaysOfWeek { get; set; } = new(); // 0=Sunday, 6=Saturday
            public int UserId { get; set; }
        }

        /// <summary>
        /// Create a new reminder
        /// </summary>
        public static Reminder CreateReminder(
            string title,
            string message,
            string reminderType,
            TimeSpan time,
            int userId)
        {
            var reminder = new Reminder
            {
                Id = _reminders.Any() ? _reminders.Max(r => r.Id) + 1 : 1,
                Title = title,
                Message = message,
                ReminderType = reminderType,
                Time = time,
                UserId = userId
            };

            _reminders.Add(reminder);
            return reminder;
        }

        /// <summary>
        /// Get all reminders for a user
        /// </summary>
        public static List<Reminder> GetUserReminders(int userId)
        {
            return _reminders.Where(r => r.UserId == userId).ToList();
        }

        /// <summary>
        /// Get enabled reminders
        /// </summary>
        public static List<Reminder> GetEnabledReminders(int userId)
        {
            return _reminders.Where(r => r.UserId == userId && r.IsEnabled).ToList();
        }

        /// <summary>
        /// Update reminder
        /// </summary>
        public static void UpdateReminder(Reminder reminder)
        {
            var existing = _reminders.FirstOrDefault(r => r.Id == reminder.Id);
            if (existing != null)
            {
                existing.Title = reminder.Title;
                existing.Message = reminder.Message;
                existing.Time = reminder.Time;
                existing.IsEnabled = reminder.IsEnabled;
                existing.DaysOfWeek = reminder.DaysOfWeek;
            }
        }

        /// <summary>
        /// Delete reminder
        /// </summary>
        public static void DeleteReminder(int reminderId)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
            if (reminder != null)
            {
                _reminders.Remove(reminder);
            }
        }

        /// <summary>
        /// Toggle reminder enabled/disabled
        /// </summary>
        public static void ToggleReminder(int reminderId)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
            if (reminder != null)
            {
                reminder.IsEnabled = !reminder.IsEnabled;
            }
        }

        /// <summary>
        /// Get reminders for a specific time
        /// </summary>
        public static List<Reminder> GetRemindersForTime(TimeSpan time, int userId)
        {
            return _reminders
                .Where(r => r.UserId == userId && r.IsEnabled && r.Time == time)
                .ToList();
        }

        /// <summary>
        /// Get reminders by type
        /// </summary>
        public static List<Reminder> GetRemindersByType(string reminderType, int userId)
        {
            return _reminders
                .Where(r => r.UserId == userId &&
                           r.ReminderType.Equals(reminderType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

using System;

namespace CalCount.Models
{
    public class CalorieEntry
    {
        public string Name { get; set; } = string.Empty;
        public int Calories { get; set; }
        public string Meal { get; set; } = "Other"; // Breakfast, Lunch, Dinner, Snack, Other
        public DateTime Date { get; set; } = DateTime.Now.Date;
    }
}

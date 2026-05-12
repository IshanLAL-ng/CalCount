namespace CalCount.View;

[QueryProperty(nameof(SelectedDate), "date")]
public partial class AddFoodPage : ContentPage
{
    private DateTime _selectedDate = DateTime.Now.Date;

    public AddFoodPage()
    {
        InitializeComponent();
    }

    public string SelectedDate
    {
        set
        {
            if (!string.IsNullOrEmpty(value) && DateTime.TryParse(Uri.UnescapeDataString(value), out var date))
            {
                _selectedDate = date.Date;
                System.Diagnostics.Debug.WriteLine($"[AddFood] Received selected date: {_selectedDate:yyyy-MM-dd}");
            }
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        var name = NameEntry?.Text ?? string.Empty;
        var caloriesText = CaloriesEntry?.Text ?? "0";
        int.TryParse(caloriesText, out var calories);
        var meal = MealPicker?.SelectedItem as string ?? "Breakfast";

        if (calories == 0)
        {
            await DisplayAlert("Validation", "Please enter valid calories.", "OK");
            return;
        }

        var entry = new Models.CalorieEntry
        {
            Name = name,
            Calories = calories,
            Meal = meal,
            Date = _selectedDate
        };

        // Save to a simple in-memory store on the App class
        var app = Application.Current as App;
        app?.AddCalorieEntry(entry);

        System.Diagnostics.Debug.WriteLine($"[AddFood] Added entry: {name}, {calories} cal, {meal}, {_selectedDate:yyyy-MM-dd}");
        System.Diagnostics.Debug.WriteLine($"[AddFood] Total entries in app after add: {app?.CalorieEntries.Count ?? 0}");

        await DisplayAlert("Saved", $"Added {calories} cal to {meal}.", "OK");

        // Clear the form for next entry
        NameEntry.Text = string.Empty;
        CaloriesEntry.Text = string.Empty;

        // Navigate back to dashboard to refresh the graphs with the new entry
        await Shell.Current.GoToAsync("dashboard");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // select default meal if none chosen
        if (MealPicker?.SelectedIndex == -1)
            MealPicker.SelectedIndex = 0; // Breakfast
    }
}

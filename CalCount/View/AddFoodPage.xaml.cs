namespace CalCount.View;

public partial class AddFoodPage : ContentPage
{
    public AddFoodPage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        var name = NameEntry?.Text ?? string.Empty;
        var caloriesText = CaloriesEntry?.Text ?? "0";
        int.TryParse(caloriesText, out var calories);
        var meal = MealPicker?.SelectedItem as string ?? "Breakfast";

        var entry = new Models.CalorieEntry
        {
            Name = name,
            Calories = calories,
            Meal = meal,
            Date = DateTime.Now.Date
        };

        // Save to a simple in-memory store on the App class (for demo)
        (Application.Current as App)?.AddCalorieEntry(entry);

        await DisplayAlert("Saved", "Entry saved.", "OK");
        await Navigation.PopAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // select default meal if none chosen
        if (MealPicker?.SelectedIndex == -1)
            MealPicker.SelectedIndex = 0; // Breakfast
    }
}

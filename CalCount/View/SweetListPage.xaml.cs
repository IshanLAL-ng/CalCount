namespace CalCount.View;

public partial class SweetListPage : ContentPage
{
    public SweetListPage()
    {
        InitializeComponent();
        BindingContext = new ViewModel.RecipesViewModel(); // Updated binding context
    }

    private async void OnRecipeClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            string title = btn.Text ?? "Sweet Dish";
            // Open a read-only RecipeDetailPage with preset content via Shell route
            await Shell.Current.GoToAsync($"recipeDetail?title={Uri.EscapeDataString(title)}");
        }
    }
}

namespace CalCount.View;

public partial class SavoryListPage : ContentPage
{
    public SavoryListPage()
    {
        InitializeComponent();
        BindingContext = new ViewModel.RecipesViewModel();
    }

    private async void OnRecipeClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            string title = btn.Text ?? "Savory Dish";
            // Open a read-only RecipeDetailPage with preset content via Shell route
            await Shell.Current.GoToAsync($"recipeDetail?title={Uri.EscapeDataString(title)}");
        }
    }
}

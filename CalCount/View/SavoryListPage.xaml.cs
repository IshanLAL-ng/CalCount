namespace CalCount.View;

public partial class SavoryListPage : ContentPage
{
    public SavoryListPage()
    {
        InitializeComponent();
    }

    private async void OnRecipeClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            string title = btn.Text ?? "Savory Dish";
            // Open a read-only RecipeDetailPage with preset content
            await Navigation.PushAsync(new RecipeDetailPage(title));
        }
    }
}

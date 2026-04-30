namespace CalCount.View;

public partial class SweetListPage : ContentPage
{
    public SweetListPage()
    {
        InitializeComponent();
    }

    private async void OnRecipeClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            string title = btn.Text ?? "Sweet Dish";
            // Open a read-only RecipeDetailPage with preset content
            await Navigation.PushAsync(new RecipeDetailPage(title));
        }
    }
}

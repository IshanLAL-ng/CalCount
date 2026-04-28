namespace CalCount.View;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(string username)
    {
        InitializeComponent();
        WelcomeLabel.Text = $"Welcome, {username}!";
        // Add a Recipes button programmatically in case editing the XAML is problematic
        if (this.Content is ScrollView sv && sv.Content is VerticalStackLayout vsl)
        {
            var recipesBtn = new Button
            {
                Text = "Recipes",
                HorizontalOptions = LayoutOptions.Center
            };

            recipesBtn.Clicked += OnRecipesClicked;
            vsl.Children.Add(recipesBtn);
        }
    }

    private async void OnRecipesClicked(object? sender, EventArgs e)
    {
        // Navigate to the recipes page
        await Navigation.PushAsync(new RecipesPage());
    }
}


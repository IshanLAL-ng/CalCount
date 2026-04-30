using Microsoft.Maui.Controls;

namespace CalCount.View;

[QueryProperty(nameof(Username), "username")]
public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();

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

    private string username = string.Empty;

    public string Username
    {
        get => username;
        set
        {
            username = value;
            // Update UI when query property is set
            if (!string.IsNullOrEmpty(username) && WelcomeLabel != null)
            {
                WelcomeLabel.Text = $"Welcome, {username}!";
            }
        }
    }

    private async void OnRecipesClicked(object? sender, EventArgs e)
    {
        // Use Shell navigation so routes are resolved consistently
        await Shell.Current.GoToAsync("recipes");
    }
}


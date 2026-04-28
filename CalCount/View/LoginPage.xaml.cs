namespace CalCount.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        var username = UsernameEntry?.Text ?? string.Empty;
        var password = PasswordEntry?.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Login failed", "Please enter both username and password.", "OK");
            return;
        }

        // Navigate to DashboardPage and set it as main page
        Application.Current.MainPage = new NavigationPage(new DashboardPage(username));
    }
}

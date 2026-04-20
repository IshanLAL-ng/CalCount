namespace CalCount.View;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private async void OnLoginClicked(object? sender, EventArgs e)
	{
		// Read values from the named Entry controls in XAML
		var username = UsernameEntry?.Text ?? string.Empty;
		var password = PasswordEntry?.Text ?? string.Empty;

		// Simple validation - replace with real auth as needed
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert("Login failed", "Please enter both username and password.", "OK");
			return;
		}

        // Example: accept any non-empty credentials
		await DisplayAlert("Login", $"Welcome, {username}!", "OK");

		// Switch to AppShell and navigate to the games landing page
		Application.Current.MainPage = new AppShell();
		// Ensure the shell is loaded then navigate to Games page
		await Shell.Current.GoToAsync("//Games");
	}
}

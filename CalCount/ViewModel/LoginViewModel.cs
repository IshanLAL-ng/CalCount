using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CalCount.ViewModel;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = new Models.Entities.LoginEntity().Title;
    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Login failed", "Please enter both username and password.", "OK");
            return;
        }

        // Validate against SQLite repository via App's service provider
        try
        {
            var sp = (Application.Current as App)?.GetServices();
            var repository = sp?.GetService(typeof(Services.IDatabaseRepository)) as Services.IDatabaseRepository;
            if (repository != null)
            {
                await repository.InitializeAsync();
                var ok = await repository.ValidateCredentialsAsync(Username, Password);
                if (!ok)
                {
                    await Shell.Current.DisplayAlert("Login failed", "Invalid username or password.", "OK");
                    return;
                }
            }
        }
        catch
        {
            // If DI resolution fails, fall back to allowing login for now (or handle appropriately)
        }

        // Navigate to dashboard using Shell route and pass username as a query parameter
        await Shell.Current.GoToAsync($"dashboard?username={Uri.EscapeDataString(Username)}");
    }
}

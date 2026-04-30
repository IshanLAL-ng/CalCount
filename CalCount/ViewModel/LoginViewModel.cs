using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CalCount.ViewModel;

public partial class LoginViewModel : ObservableObject
{
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

        // Navigate to dashboard using Shell route and pass username as a query parameter
        await Shell.Current.GoToAsync($"dashboard?username={Uri.EscapeDataString(Username)}");
    }
}

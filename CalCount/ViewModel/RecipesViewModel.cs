using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CalCount.ViewModel;

public partial class RecipesViewModel : ObservableObject
{
    [RelayCommand]
    private async Task OpenSavoryAsync()
    {
        await Shell.Current.GoToAsync("savory");
    }

    [RelayCommand]
    private async Task OpenSweetAsync()
    {
        await Shell.Current.GoToAsync("sweet");
    }
}

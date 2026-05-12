using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = new Models.Entities.SettingsEntity().Title;
}

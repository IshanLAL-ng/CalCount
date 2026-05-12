using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = new Models.Entities.DashboardEntity().Title;
}

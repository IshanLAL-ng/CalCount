using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class SweetListViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = new Models.Entities.SweetListEntity().Title;
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class SavoryListViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = new Models.Entities.SavoryListEntity().Title;
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class AddFoodViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = new Models.Entities.AddFoodEntity().Title;
}

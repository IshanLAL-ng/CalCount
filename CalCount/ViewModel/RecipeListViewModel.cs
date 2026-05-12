using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class RecipeListViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = new Models.Entities.RecipesEntity().Title;
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class RecipeDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string imageUrl = string.Empty;

    [ObservableProperty]
    private string ingredients = string.Empty;

    [ObservableProperty]
    private string instructions = string.Empty;

    public void LoadSample(string recipeTitle)
    {
        Title = recipeTitle;
        ImageUrl = "https://via.placeholder.com/300x200.png?text=Recipe+Image";
        Ingredients = "- 1 cup sample ingredient\n- 2 tbsp another ingredient";
        Instructions = "1. Do this.\n2. Do that.\n3. Serve hot.";
    }
}

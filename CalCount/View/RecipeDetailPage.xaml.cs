namespace CalCount.View;

[QueryProperty(nameof(Title), "title")]
public partial class RecipeDetailPage : ContentPage
{
    private readonly ViewModel.RecipeDetailViewModel vm;
    private string title = string.Empty;

    public RecipeDetailPage()
    {
        InitializeComponent();
        vm = new ViewModel.RecipeDetailViewModel(); // Initialize ViewModel
        BindingContext = vm; // Set BindingContext to ViewModel
    }

    public string Title
    {
        get => title;
        set
        {
            title = Uri.UnescapeDataString(value ?? string.Empty);
            vm.LoadSample(title);
        }
    }
}

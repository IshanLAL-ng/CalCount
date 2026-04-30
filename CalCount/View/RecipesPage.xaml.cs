namespace CalCount.View;

public partial class RecipesPage : ContentPage
{
    public RecipesPage()
    {
        InitializeComponent();
    }

    private async void OnSavoryClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SavoryListPage());
    }

    private async void OnSweetClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SweetListPage());
    }
}

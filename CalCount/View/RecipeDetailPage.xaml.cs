namespace CalCount.View;

public partial class RecipeDetailPage : ContentPage
{
    public RecipeDetailPage(string defaultTitle)
    {
        InitializeComponent();
        // Set read-only fields from the default title
        TitleLabel.Text = defaultTitle;

        // Hard-coded placeholder image (embedded resource or file path can be used). For now use a simple built-in URI or leave empty.
        RecipeImage.Source = "https://via.placeholder.com/300x200.png?text=Recipe+Image";

        // Optionally populate sample ingredients/instructions for demonstration
        IngredientsLabel.Text = "- 1 cup sample ingredient\n- 2 tbsp another ingredient";
        InstructionsLabel.Text = "1. Do this.\n2. Do that.\n3. Serve hot.";
    }

}

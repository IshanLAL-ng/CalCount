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
        // provide specific sample content per known recipe titles
        switch ((recipeTitle ?? string.Empty).Trim())
        {
            case "Grilled Chicken":
                ImageUrl = "grilled_chicken.svg";
                Ingredients = "- 2 chicken breasts\n- 1 tbsp olive oil\n- 1 tsp paprika\n- Salt and pepper to taste";
                Instructions = "1. Preheat grill to medium-high.\n2. Brush chicken with olive oil and season.\n3. Grill 6-7 minutes per side until cooked through.\n4. Let rest and serve.";
                break;
            case "Beef Stew":
                ImageUrl = "beef_stew.svg";
                Ingredients = "- 1 lb beef chuck, cubed\n- 2 carrots, sliced\n- 2 potatoes, cubed\n- 1 onion, chopped\n- 4 cups beef broth";
                Instructions = "1. Brown beef in a pot.\n2. Add vegetables and broth.\n3. Simmer 1.5-2 hours until beef is tender.\n4. Season and serve.";
                break;
            case "Vegetable Stir Fry":
                ImageUrl = "vegetable_stirfry.svg";
                Ingredients = "- Assorted vegetables (broccoli, bell pepper, carrot)\n- 2 tbsp soy sauce\n- 1 tbsp sesame oil\n- 1 clove garlic, minced";
                Instructions = "1. Heat oil in a wok.\n2. Add garlic and vegetables, stir-fry until tender-crisp.\n3. Add soy sauce, toss and serve over rice.";
                break;
            case "Chocolate Cake":
                ImageUrl = "chocolate_cake.svg";
                Ingredients = "- 1.5 cups flour\n- 1 cup sugar\n- 1/2 cup cocoa powder\n- 1 tsp baking powder\n- 2 eggs";
                Instructions = "1. Preheat oven to 350°F (175°C).\n2. Mix dry and wet ingredients.\n3. Bake 30-35 minutes.\n4. Cool and frost as desired.";
                break;
            case "Apple Pie":
                ImageUrl = "apple_pie.svg";
                Ingredients = "- 6 cups thinly sliced apples\n- 3/4 cup sugar\n- 2 tbsp flour\n- 1 tsp cinnamon\n- Pie crust";
                Instructions = "1. Preheat oven to 425°F (220°C).\n2. Toss apples with sugar, flour and cinnamon.\n3. Fill crust, cover and bake 40-45 minutes.";
                break;
            case "Pancakes":
                ImageUrl = "pancakes.svg";
                Ingredients = "- 1 cup flour\n- 1 cup milk\n- 1 egg\n- 1 tbsp sugar\n- 1 tsp baking powder";
                Instructions = "1. Mix ingredients until smooth.\n2. Pour batter on a hot griddle.\n3. Cook until bubbles form, flip and cook until golden.";
                break;
            default:
                ImageUrl = "recipe_image.png"; // fallback local name; replace if needed
                Ingredients = "- 1 cup sample ingredient\n- 2 tbsp another ingredient";
                Instructions = "1. Do this.\n2. Do that.\n3. Serve hot.";
                break;
        }
    }
}

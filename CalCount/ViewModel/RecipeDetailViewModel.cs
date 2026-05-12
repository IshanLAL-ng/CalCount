using CommunityToolkit.Mvvm.ComponentModel;

namespace CalCount.ViewModel;

public partial class RecipeDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private string titleText = new Models.Entities.RecipeDetailEntity().Title;
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
                Ingredients = "- 2 chicken breasts\n- 1 tbsp olive oil\n- 1 tsp paprika\n- Salt and pepper to taste\n- Lemon juice";
                Instructions = "1. Preheat grill to medium-high heat (375-400°F).\n2. Brush chicken breasts with olive oil and season with paprika, salt, and pepper.\n3. Grill 6-7 minutes per side until cooked through (internal temp 165°F).\n4. Squeeze fresh lemon juice over chicken.\n5. Let rest for 2-3 minutes and serve.";
                break;
            case "Beef Stew":
                ImageUrl = "beef_stew.svg";
                Ingredients = "- 1 lb beef chuck, cubed\n- 2 carrots, sliced\n- 2 potatoes, cubed\n- 1 onion, chopped\n- 4 cups beef broth\n- Tomato paste, garlic";
                Instructions = "1. Heat oil in a large pot and brown beef cubes on all sides.\n2. Add chopped onions and sauté until soft.\n3. Stir in tomato paste and garlic.\n4. Add vegetables and beef broth.\n5. Bring to a boil, then simmer 1.5-2 hours until beef is tender.\n6. Season with salt and pepper to taste.\n7. Serve hot.";
                break;
            case "Vegetable Stir Fry":
                ImageUrl = "vegetable_stirfry.svg";
                Ingredients = "- 2 cups broccoli florets\n- 1 bell pepper, sliced\n- 1 carrot, sliced\n- 2 cloves garlic, minced\n- 2 tbsp soy sauce\n- 1 tbsp sesame oil\n- 1 tsp ginger";
                Instructions = "1. Heat sesame oil in a wok or large skillet over high heat.\n2. Add garlic and ginger, stir-fry for 30 seconds.\n3. Add carrot and broccoli, stir-fry for 3-4 minutes.\n4. Add bell pepper and continue stir-frying.\n5. Pour in soy sauce and toss well.\n6. Cook until vegetables are tender-crisp (2-3 minutes more).\n7. Serve over rice.";
                break;
            case "Chocolate Cake":
                ImageUrl = "chocolate_cake.svg";
                Ingredients = "- 1.5 cups flour\n- 1 cup sugar\n- 1/2 cup cocoa powder\n- 1 tsp baking powder\n- 1/2 tsp baking soda\n- 2 eggs\n- 1/2 cup milk\n- 1/3 cup oil";
                Instructions = "1. Preheat oven to 350°F (175°C). Grease a 9-inch round pan.\n2. Mix flour, sugar, cocoa powder, baking powder, and baking soda.\n3. In another bowl, beat eggs and combine with milk and oil.\n4. Mix wet and dry ingredients until smooth.\n5. Pour into prepared pan.\n6. Bake 30-35 minutes until a toothpick comes out clean.\n7. Cool completely before frosting.";
                break;
            case "Apple Pie":
                ImageUrl = "apple_pie.svg";
                Ingredients = "- 6 cups thinly sliced apples\n- 3/4 cup sugar\n- 2 tbsp flour\n- 1 tsp cinnamon\n- 1/4 tsp nutmeg\n- 1 pre-made pie crust\n- 1 tbsp lemon juice";
                Instructions = "1. Preheat oven to 425°F (220°C).\n2. Toss apple slices with sugar, flour, cinnamon, nutmeg, and lemon juice.\n3. Pour mixture into pie crust.\n4. Cover with foil and bake for 20 minutes.\n5. Remove foil and bake 20-25 minutes more until crust is golden.\n6. Cool for 15 minutes before serving.";
                break;
            case "Pancakes":
                ImageUrl = "pancakes.svg";
                Ingredients = "- 1 cup all-purpose flour\n- 1 cup milk\n- 1 egg\n- 1 tbsp sugar\n- 1 tsp baking powder\n- 1/2 tsp salt\n- Butter for cooking";
                Instructions = "1. In a bowl, whisk together flour, sugar, baking powder, and salt.\n2. In another bowl, beat egg and combine with milk.\n3. Mix wet and dry ingredients until smooth.\n4. Heat butter on a griddle or pan over medium heat.\n5. Pour 1/4 cup batter for each pancake.\n6. Cook until bubbles form on surface (1-2 minutes), then flip.\n7. Cook until golden brown on other side.\n8. Serve with syrup and butter.";
                break;
            default:
                ImageUrl = "grilled_chicken.svg"; // fallback to grilled chicken
                Ingredients = "- 1 cup sample ingredient\n- 2 tbsp another ingredient";
                Instructions = "1. Do this.\n2. Do that.\n3. Serve hot.";
                break;
        }
    }
}

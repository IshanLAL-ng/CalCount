using System.Collections.ObjectModel;
using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class RecipesViewModel : BaseViewModel
    {
        private int _userId = 1;
        private string _recipeName = string.Empty;
        private string _recipeType = "Savory";
        private string _ingredients = string.Empty;
        private string _instructions = string.Empty;
        private int _prepTime = 15;
        private int _cookTime = 30;
        private int _servings = 4;
        private Recipe? _selectedRecipe;
        private string _searchQuery = string.Empty;

        public string RecipeName
        {
            get => _recipeName;
            set => SetProperty(ref _recipeName, value);
        }

        public string RecipeType
        {
            get => _recipeType;
            set => SetProperty(ref _recipeType, value);
        }

        public string Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        public string Instructions
        {
            get => _instructions;
            set => SetProperty(ref _instructions, value);
        }

        public int PrepTime
        {
            get => _prepTime;
            set => SetProperty(ref _prepTime, value);
        }

        public int CookTime
        {
            get => _cookTime;
            set => SetProperty(ref _cookTime, value);
        }

        public int Servings
        {
            get => _servings;
            set => SetProperty(ref _servings, value);
        }

        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set => SetProperty(ref _selectedRecipe, value);
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public ObservableCollection<string> RecipeTypes { get; set; } = new() { "Savory", "Sweet", "Beverage", "Snack", "Breakfast", "Lunch", "Dinner", "Dessert" };
        public ObservableCollection<Recipe> AllRecipes { get; set; } = new();
        public ObservableCollection<Recipe> SearchResults { get; set; } = new();

        public RecipesViewModel()
        {
            Title = "Recipes";
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            AllRecipes.Clear();
            var recipes = RecipeService.GetUserRecipes(_userId);
            foreach (var recipe in recipes)
            {
                AllRecipes.Add(recipe);
            }
        }

        public void SearchRecipes()
        {
            SearchResults.Clear();

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                foreach (var recipe in AllRecipes)
                {
                    SearchResults.Add(recipe);
                }
                return;
            }

            var results = RecipeService.SearchRecipes(SearchQuery, _userId);
            foreach (var recipe in results)
            {
                SearchResults.Add(recipe);
            }
        }

        public void FilterByType(string type)
        {
            SearchResults.Clear();
            var recipes = RecipeService.GetRecipesByType(type, _userId);
            foreach (var recipe in recipes)
            {
                SearchResults.Add(recipe);
            }
        }

        public void SelectRecipe(Recipe recipe)
        {
            SelectedRecipe = recipe;
            RecipeName = recipe.Name;
            RecipeType = recipe.Type;
            Ingredients = recipe.Ingredients ?? string.Empty;
            Instructions = recipe.Instructions ?? string.Empty;
            PrepTime = recipe.PrepTimeMinutes;
            CookTime = recipe.CookTimeMinutes;
            Servings = recipe.Servings;
        }

        public void SaveRecipe()
        {
            if (string.IsNullOrWhiteSpace(RecipeName))
                return;

            if (SelectedRecipe == null)
            {
                var newRecipe = RecipeService.CreateRecipe(RecipeName, RecipeType, _userId);
                newRecipe.Ingredients = Ingredients;
                newRecipe.Instructions = Instructions;
                newRecipe.PrepTimeMinutes = PrepTime;
                newRecipe.CookTimeMinutes = CookTime;
                newRecipe.Servings = Servings;
                RecipeService.UpdateRecipe(newRecipe);
            }
            else
            {
                SelectedRecipe.Name = RecipeName;
                SelectedRecipe.Type = RecipeType;
                SelectedRecipe.Ingredients = Ingredients;
                SelectedRecipe.Instructions = Instructions;
                SelectedRecipe.PrepTimeMinutes = PrepTime;
                SelectedRecipe.CookTimeMinutes = CookTime;
                SelectedRecipe.Servings = Servings;
                RecipeService.UpdateRecipe(SelectedRecipe);
            }

            LoadRecipes();
            ResetForm();
        }

        public void DeleteRecipe(Recipe recipe)
        {
            RecipeService.DeleteRecipe(recipe.Id);
            LoadRecipes();
        }

        public void UseRecipeAsMeal(Recipe recipe)
        {
            // This would integrate with food logging
            // For now, just show notification
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert("Recipe", $"Using {recipe.Name} as a meal", "OK")!;
            });
        }

        private void ResetForm()
        {
            RecipeName = string.Empty;
            RecipeType = "Savory";
            Ingredients = string.Empty;
            Instructions = string.Empty;
            PrepTime = 15;
            CookTime = 30;
            Servings = 4;
            SelectedRecipe = null;
        }
    }
}

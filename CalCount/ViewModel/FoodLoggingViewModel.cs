using System.Collections.ObjectModel;
using CalCount.Models;
using CalCount.Services;

namespace CalCount.ViewModel
{
    public class FoodLoggingViewModel : BaseViewModel
    {
        private int _userId = 1;
        private string _searchQuery = string.Empty;
        private double _selectedFoodQuantity = 1;
        private Food? _selectedFood;
        private string _selectedUnit = "g";
        private ObservableCollection<string> _availableUnits = new() { "g", "oz", "piece", "cup", "tbsp" };
        private bool _isFavorite = false;

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public double SelectedFoodQuantity
        {
            get => _selectedFoodQuantity;
            set => SetProperty(ref _selectedFoodQuantity, value);
        }

        public Food? SelectedFood
        {
            get => _selectedFood;
            set => SetProperty(ref _selectedFood, value);
        }

        public string SelectedUnit
        {
            get => _selectedUnit;
            set => SetProperty(ref _selectedUnit, value);
        }

        public ObservableCollection<string> AvailableUnits
        {
            get => _availableUnits;
            set => SetProperty(ref _availableUnits, value);
        }

        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        public ObservableCollection<Food> SearchResults { get; set; } = new();
        public ObservableCollection<Food> FavoriteFoods { get; set; } = new();
        public ObservableCollection<string> Categories { get; set; } = new();

        public FoodLoggingViewModel()
        {
            Title = "Log Food";
            LoadCategories();
            LoadFavoriteFoods();
        }

        private void LoadCategories()
        {
            Categories.Clear();
            var cats = FoodDatabaseService.GetAllCategories();
            foreach (var cat in cats)
            {
                Categories.Add(cat);
            }
        }

        private void LoadFavoriteFoods()
        {
            FavoriteFoods.Clear();
            var favorites = FoodDatabaseService.GetFavoriteFoods();
            foreach (var food in favorites)
            {
                FavoriteFoods.Add(food);
            }
        }

        public void SearchFoods()
        {
            SearchResults.Clear();

            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            var results = FoodDatabaseService.SearchFoodByName(SearchQuery);
            foreach (var food in results)
            {
                SearchResults.Add(food);
            }
        }

        public void FilterByCategory(string category)
        {
            SearchResults.Clear();
            var results = FoodDatabaseService.GetFoodsByCategory(category);
            foreach (var food in results)
            {
                SearchResults.Add(food);
            }
        }

        public void SelectFood(Food food)
        {
            SelectedFood = food;
            SelectedFoodQuantity = 1;
            SelectedUnit = "g";
            IsFavorite = food.IsFavorite;
        }

        public void LogFood()
        {
            if (SelectedFood?.Nutrition == null)
                return;

            var food = new Food
            {
                Id = SelectedFood.Id,
                Name = SelectedFood.Name,
                Category = SelectedFood.Category,
                Nutrition = SelectedFood.Nutrition,
                Quantity = SelectedFoodQuantity,
                Unit = SelectedUnit,
                LoggedDateTime = DateTime.Now,
                UserId = _userId,
                IsFavorite = IsFavorite
            };

            var logs = LocalStorageService.LoadFoodLogs();
            logs.Add(food);
            LocalStorageService.SaveFoodLogs(logs);

            // Update favorite status
            if (IsFavorite != SelectedFood.IsFavorite)
            {
                FoodDatabaseService.ToggleFavorite(SelectedFood.Id);
                LoadFavoriteFoods();
            }

            ResetForm();
        }

        public void ToggleFavorite(Food food)
        {
            FoodDatabaseService.ToggleFavorite(food.Id);
            LoadFavoriteFoods();
        }

        private void ResetForm()
        {
            SelectedFood = null;
            SelectedFoodQuantity = 1;
            SelectedUnit = "g";
            SearchQuery = string.Empty;
            SearchResults.Clear();
            IsFavorite = false;
        }

        public void ScanBarcode(string barcode)
        {
            var food = FoodDatabaseService.SearchFoodByBarcode(barcode);
            if (food != null)
            {
                SelectFood(food);
            }
        }
    }
}

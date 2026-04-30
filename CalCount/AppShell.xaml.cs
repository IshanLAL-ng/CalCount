namespace CalCount
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for pages navigated via Shell
            Routing.RegisterRoute("savory", typeof(View.SavoryListPage));
            Routing.RegisterRoute("sweet", typeof(View.SweetListPage));
            Routing.RegisterRoute("recipeDetail", typeof(View.RecipeDetailPage));
            Routing.RegisterRoute("recipes", typeof(View.RecipesPage));
            Routing.RegisterRoute("dashboard", typeof(View.DashboardPage));
            Routing.RegisterRoute("login", typeof(View.LoginPage));
        }
    }
}

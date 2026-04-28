using Microsoft.Extensions.DependencyInjection;

namespace CalCount
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Start the app inside a NavigationPage so pages can push/pop and show the back button
            return new Window(new NavigationPage(new View.LoginPage()));
        }
    }
}
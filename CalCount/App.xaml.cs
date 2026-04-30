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
            // Use AppShell as the application's main Shell so Shell.Current is available
            return new Window(new AppShell());
        }
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace CalCount
{
    public partial class App : Application
    {
        private readonly List<Models.CalorieEntry> _entries = new();

        public IReadOnlyList<Models.CalorieEntry> CalorieEntries => _entries.AsReadOnly();

        public void AddCalorieEntry(Models.CalorieEntry entry)
        {
            _entries.Add(entry);
        }

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
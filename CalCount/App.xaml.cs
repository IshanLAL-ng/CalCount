using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace CalCount
{
    public partial class App : Application
    {
        private readonly List<Models.CalorieEntry> _entries = new();
        private const string EntriesFileName = "calorie_entries.json";

        public IReadOnlyList<Models.CalorieEntry> CalorieEntries => _entries.AsReadOnly();

        public void AddCalorieEntry(Models.CalorieEntry entry)
        {
            _entries.Add(entry);
            SaveEntries();
        }

        public void RemoveEntriesForDate(DateTime date)
        {
            _entries.RemoveAll(e => e.Date.Date == date.Date);
            SaveEntries();
        }

        private void SaveEntries()
        {
            try
            {
                var json = JsonSerializer.Serialize(_entries);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, EntriesFileName);
                File.WriteAllText(filePath, json);
                System.Diagnostics.Debug.WriteLine($"[App] Saved {_entries.Count} entries to {filePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[App] Error saving entries: {ex.Message}");
            }
        }

        private void LoadEntries()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, EntriesFileName);
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var loaded = JsonSerializer.Deserialize<List<Models.CalorieEntry>>(json);
                    if (loaded != null)
                    {
                        _entries.Clear();
                        _entries.AddRange(loaded);
                        System.Diagnostics.Debug.WriteLine($"[App] Loaded {_entries.Count} entries from {filePath}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[App] No saved entries file found at {filePath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[App] Error loading entries: {ex.Message}");
            }
        }

        public App()
        {
            InitializeComponent();
            LoadEntries();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Use AppShell as the application's main Shell so Shell.Current is available
            return new Window(new AppShell());
        }
    }
}
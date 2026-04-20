using CalCount.Models;

namespace CalCount.Services
{
    /// <summary>
    /// Service for local data storage and persistence
    /// In production, this would use SQLite or a similar database
    /// </summary>
    public class LocalStorageService
    {
        private static readonly Dictionary<string, List<object>> DataStore = new();

        /// <summary>
        /// Save data to local storage
        /// </summary>
        public static void SaveData<T>(string key, List<T> data) where T : class
        {
            if (DataStore.ContainsKey(key))
            {
                DataStore[key] = data.Cast<object>().ToList();
            }
            else
            {
                DataStore.Add(key, data.Cast<object>().ToList());
            }
        }

        /// <summary>
        /// Load data from local storage
        /// </summary>
        public static List<T> LoadData<T>(string key) where T : class
        {
            if (DataStore.TryGetValue(key, out var data))
            {
                return data.Cast<T>().ToList();
            }

            return new List<T>();
        }

        /// <summary>
        /// Save food logs
        /// </summary>
        public static void SaveFoodLogs(List<Food> foods)
        {
            SaveData("food_logs", foods);
        }

        /// <summary>
        /// Load food logs
        /// </summary>
        public static List<Food> LoadFoodLogs()
        {
            return LoadData<Food>("food_logs");
        }

        /// <summary>
        /// Save workout logs
        /// </summary>
        public static void SaveWorkoutLogs(List<Workout> workouts)
        {
            SaveData("workout_logs", workouts);
        }

        /// <summary>
        /// Load workout logs
        /// </summary>
        public static List<Workout> LoadWorkoutLogs()
        {
            return LoadData<Workout>("workout_logs");
        }

        /// <summary>
        /// Save water logs
        /// </summary>
        public static void SaveWaterLogs(List<WaterLog> waterLogs)
        {
            SaveData("water_logs", waterLogs);
        }

        /// <summary>
        /// Load water logs
        /// </summary>
        public static List<WaterLog> LoadWaterLogs()
        {
            return LoadData<WaterLog>("water_logs");
        }

        /// <summary>
        /// Save progress data
        /// </summary>
        public static void SaveProgressData(List<ProgressData> progress)
        {
            SaveData("progress_data", progress);
        }

        /// <summary>
        /// Load progress data
        /// </summary>
        public static List<ProgressData> LoadProgressData()
        {
            return LoadData<ProgressData>("progress_data");
        }

        /// <summary>
        /// Save user profile
        /// </summary>
        public static void SaveUserProfile(UserProfile profile)
        {
            SaveData("user_profile", new List<UserProfile> { profile });
        }

        /// <summary>
        /// Load user profile
        /// </summary>
        public static UserProfile? LoadUserProfile()
        {
            var profiles = LoadData<UserProfile>("user_profile");
            return profiles.FirstOrDefault();
        }

        /// <summary>
        /// Clear all data
        /// </summary>
        public static void ClearAllData()
        {
            DataStore.Clear();
        }

        /// <summary>
        /// Delete specific data set
        /// </summary>
        public static void DeleteData(string key)
        {
            if (DataStore.ContainsKey(key))
            {
                DataStore.Remove(key);
            }
        }
    }
}

using CalCount.Models.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CalCount.Services;

public interface IDatabaseRepository
{
    // initialize database and tables
    Task InitializeAsync();

    // user operations
    Task<User?> GetUserByUsernameAsync(string username);
    Task<int> AddUserAsync(User user);
    Task<bool> ValidateCredentialsAsync(string username, string password);

    // optional: list users (for admin/testing)
    Task<List<User>> GetAllUsersAsync();
}

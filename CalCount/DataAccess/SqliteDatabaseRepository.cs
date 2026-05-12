using SQLite;
using BCrypt.Net;
using CalCount.Models.Entities;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System;

namespace CalCount.Services;

public class SqliteDatabaseRepository : IDatabaseRepository
{
    readonly SQLiteAsyncConnection _db;

    public SqliteDatabaseRepository()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "calcount.db3");
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitializeAsync()
    {
        await _db.CreateTableAsync<User>();
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _db.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task<int> AddUserAsync(User user)
    {
        // hash the password before saving
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }
        return await _db.InsertAsync(user);
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await GetUserByUsernameAsync(username);
        if (user == null) return false;
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _db.Table<User>().ToListAsync();
    }
}

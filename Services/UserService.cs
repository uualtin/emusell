using MongoDB.Driver;
using Emusell.Models;
using BCrypt.Net;

namespace Emusell.Services;

public class UserService
{
    private readonly MongoDbService _mongoDb;

    public UserService(MongoDbService mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _mongoDb.Users.Find(_ => true).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _mongoDb.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _mongoDb.Users.Find(u => u.Email == email.ToLower()).FirstOrDefaultAsync();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.Email = user.Email.ToLower();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        user.CreatedAt = DateTime.UtcNow;
        await _mongoDb.Users.InsertOneAsync(user);
        return user;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var result = await _mongoDb.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var result = await _mongoDb.Users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> SuspendUserAsync(string id)
    {
        var update = Builders<User>.Update.Set(u => u.IsActive, false);
        var result = await _mongoDb.Users.UpdateOneAsync(u => u.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> ReactivateUserAsync(string id)
    {
        var update = Builders<User>.Update.Set(u => u.IsActive, true);
        var result = await _mongoDb.Users.UpdateOneAsync(u => u.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> ChangeRoleAsync(string id, UserRole role)
    {
        var update = Builders<User>.Update.Set(u => u.Role, role);
        var result = await _mongoDb.Users.UpdateOneAsync(u => u.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> ChangePasswordAsync(string id, string newPassword)
    {
        var update = Builders<User>.Update.Set(u => u.PasswordHash, BCrypt.Net.BCrypt.HashPassword(newPassword));
        var result = await _mongoDb.Users.UpdateOneAsync(u => u.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    public async Task<List<User>> GetUsersByRoleAsync(UserRole role)
    {
        return await _mongoDb.Users.Find(u => u.Role == role).ToListAsync();
    }

    public async Task<int> GetUserCountByRoleAsync(UserRole role)
    {
        return (int)await _mongoDb.Users.CountDocumentsAsync(u => u.Role == role);
    }
}

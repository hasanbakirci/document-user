using Core.Repositories.Settings;
using MongoDB.Driver;
using user_service.Models;

namespace user_service.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _user;

    public UserRepository(IMongoSettings settings)
    {
        var client = new MongoClient(settings.Server);
        var database = client.GetDatabase(settings.Database);
        _user = database.GetCollection<User>(settings.Collection);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _user.Find(d => true).ToListAsync();
    }

    public async Task<User> GetById(Guid id)
    {
        var user = await _user.FindAsync(d => d.Id == id);
        return user.FirstOrDefault();
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _user.FindAsync(d => d.Email == email);
        return user.FirstOrDefault();
    }

    public async Task<string> Create(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        await _user.InsertOneAsync(user);
        return user.Id.ToString();
    }

    public async Task<bool> Update(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        var result = await _user.FindOneAndReplaceAsync(u => u.Id == user.Id, user);
        return result is null ? false : true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await _user.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount < 1 ? false : true;
    }
}
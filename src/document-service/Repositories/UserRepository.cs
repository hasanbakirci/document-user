using Core.Repositories.Settings;
using document_service.Models;
using MongoDB.Driver;

namespace document_service.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _user;
    
    public UserRepository(IMongoSettings settings)
    {
        var client = new MongoClient(settings.Server);
        var database = client.GetDatabase(settings.Database);
        _user = database.GetCollection<User>("Users");
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
        user.UpdatedAt = DateTime.UtcNow;
        await _user.InsertOneAsync(user);
        return user.Id.ToString();
    }

    public async Task<bool> Update(Guid id,User user)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        var update = Builders<User>.Update
            .Set(u => u.Username, user.Username)
            .Set(u => u.Password, user.Password)
            .Set(u => u.Role, user.Role)
            .Set(u => u.UpdatedAt, DateTime.UtcNow);
        var result = await _user.UpdateOneAsync(filter, update);
        if (result.ModifiedCount > 0)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await _user.DeleteOneAsync(u => u.Id == id);
        if (result.DeletedCount > 0)
        {
            return true;
        }
        return false;
    }
}
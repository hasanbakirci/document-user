using core.Mongo.MongoRepository;
using document_service.Models;

namespace document_service.Repositories;

public interface IUserRepository : IMongoRepository<User>
{
    // Task<IEnumerable<User>> GetAll();
    // Task<User> GetById(Guid id);
    // Task<User> GetByEmail(string email);
    // Task<string> Create(User user);
    // Task<bool> Update(Guid id,User user);
    // Task<bool> Delete(Guid id);
}
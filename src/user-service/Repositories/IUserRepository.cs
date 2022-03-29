using user_service.Models;

namespace user_service.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string email);
    Task<string> Create(User user);
    Task<bool> Update(User user);
    Task<bool> Delete(Guid id);
}
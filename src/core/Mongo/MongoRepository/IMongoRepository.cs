using System.Linq.Expressions;

namespace core.Mongo.MongoRepository;

public interface IMongoRepository<T> where T : class
{
    List<T> FindAll();
    Task<List<T>> FindAllAsync();
    T FindById(Guid id);
    Task<T> FindByIdAsync(Guid id);
    List<T> FilterBy(Expression<Func<T, bool>> filter);
    Task<List<T>> FilterByAsync(Expression<Func<T, bool>> filter);
    T InsertOne(T document);
    Task<T> InsertOneAsync(T document);
    bool UpdateOne(Expression<Func<T, bool>> expression, params (Expression<Func<T, object>>, object)[] updatedProperties);
    Task<bool> UpdateOneAsync(Expression<Func<T, bool>> expression, params (Expression<Func<T, object>>, object)[] updatedProperties);
    bool ReplaceOne(Guid id,T document);
    Task<bool> ReplaceOneAsync(Guid id,T document);
    bool DeleteOne(Guid id);
    Task<bool> DeleteOneAsync(Guid id);
}
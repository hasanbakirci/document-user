using System.Linq.Expressions;

namespace core.Mongo.MongoRepository;

public interface IMongoRepository<T> where T : class
{
    Task<List<T>> GetAll();
    Task<T> GetBy(Expression<Func<T, bool>> expression);
    Task<List<T>> FilterBy(Expression<Func<T, bool>> filter);
    Task<T> InsertOne(T document);
    Task<float> UpdateOne(Expression<Func<T, bool>> expression, params (Expression<Func<T, object>>, object)[] updatedProperties);
    Task<float> ReplaceOne(Expression<Func<T, bool>> expression,T document);
    Task<float> DeleteOne(Expression<Func<T, bool>> expression);
}
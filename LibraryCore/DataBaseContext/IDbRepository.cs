
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibraryCore.DataBaseContext
{
    public interface IDbRepository<T>
    {
        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        Task AddAsync(T resource);
        Task<bool> UpdateAsync(string id, T resource);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}

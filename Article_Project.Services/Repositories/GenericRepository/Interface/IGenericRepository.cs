using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Services.Repositories.GenericRepository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> Expression = null,
                                            params Expression<Func<T, object>>[] Includes);

        Task<T> GetByIdAsync(object Id);

        Task<bool> AddAsync(T Item);
        Task<bool> AddRangeAsync(List<T> Items);
        Task<bool> RemoveAsync(T Item);
        Task<bool> RemoveRangeAsync(List<T> Items);
        Task<bool> RemoveByIdAsync(object Id);
        Task<bool> UpdateAsync(T Item);
        T this[int Index] { get; }

    }
}

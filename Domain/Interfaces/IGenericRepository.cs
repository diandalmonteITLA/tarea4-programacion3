using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class

    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IReadOnlyCollection<T>> GetAllAsync(bool includeInactive = false);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeactiveAsync(Guid id);
    }


}

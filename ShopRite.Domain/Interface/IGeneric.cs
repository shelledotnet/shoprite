using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Domain.Interface
{
    public interface IGeneric<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> AddAsync(TEntity entity); 
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(TEntity entity);

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopRite.Application.Exceptions;
using ShopRite.Domain.Interface;
using ShopRite.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Infrastructure.Repository
{
    //usin primary constructor
    public class GenericRepository<TEntity>(AppDbContext appDbContext) : IGeneric<TEntity> where TEntity : class
    {
        public async Task<int> AddAsync(TEntity entity)
        {
                appDbContext.Set<TEntity>().Add(entity);
                return await appDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await appDbContext.Set<TEntity>().FindAsync(id);

                if (entity != null)
                {
                    appDbContext.Set<TEntity>().Remove(entity);
                    return await appDbContext.SaveChangesAsync();
                }
                return 0;
            }    
            catch (Exception)
            {
                //TODO log Exception
                return 0;
            }

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await appDbContext.Set<TEntity>().AsNoTracking().ToListAsync();  
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await appDbContext.Set<TEntity>().FindAsync(id);

        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
           appDbContext.Set<TEntity>().Update(entity);
            return await appDbContext.SaveChangesAsync();
           
        }
    }
}

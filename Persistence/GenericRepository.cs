using System;
using Domain.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(bool includeInactive = false)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!includeInactive)
            {
                // EF.Property allows filtering by IsActive without forcing T to implement a specific interface
                query = query.Where(e => EF.Property<bool>(e, "IsActive") == true);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeactiveAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                // Accesses the tracking entry and modifies the IsActive property directly
                _context.Entry(entity).Property("IsActive").CurrentValue = false;
                await _context.SaveChangesAsync();
            }
        }

    }
}

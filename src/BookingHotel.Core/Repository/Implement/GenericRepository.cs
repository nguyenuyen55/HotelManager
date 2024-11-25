using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookingHotel.Core.Persistence;
using BookingHotel.Core.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Core.Repository.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelBookingDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(HotelBookingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

      // Implement phương thức GetAsync với predicate
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        // Implement phương thức GetAllAsync với predicate
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddListAsync(List<T> entitys)
        {
            await _dbSet.AddRangeAsync(entitys);
        }
    }

}

using BookingHotel.Core.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookingHotel.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();

          Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
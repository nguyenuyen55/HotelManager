using System.Linq.Expressions;
using BackendAPIBookingHotel.Model;
using BookingHotel.Core.Persistence;
using BookingHotel.Core.Repository.Interface;
using Microsoft.EntityFrameworkCore;

public class BookingGenericRepository : IBookingGenericRepository
{
    private readonly HotelBookingDbContext _context;

    public BookingGenericRepository(HotelBookingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Booking entity)
    {
        await _context.Bookings.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public Task AddListAsync(List<Booking> entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        var Booking = await _context.Bookings.FindAsync(id);
        if (Booking != null)
        {
            _context.Bookings.Remove(Booking);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetAllAsync(Expression<Func<Booking, bool>> predicate)
    {
        return await _context.Bookings.Where(predicate).ToListAsync();
    }

    public async Task<Booking> GetAsync(Expression<Func<Booking, bool>> predicate)
    {
        return await _context.Bookings.FirstOrDefaultAsync(predicate);
    }

    public async Task<Booking> GetByIdAsync(int id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task UpdateAsync(Booking entity)
    {
        _context.Bookings.Update(entity);
        await _context.SaveChangesAsync();
    }
}
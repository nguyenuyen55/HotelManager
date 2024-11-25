// AddressService.cs

using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.Models;

public class AddressService : IAddressService
{
    private readonly IUnitOfWork _unitOfWork;

    public AddressService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateAddressAsync(Address_InsertRequestData model)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                // Tạo mới Address
                var address = new Address
                {
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    Country = model.Country,
                    AddressType = model.AddressType,
                    IsPrimary = model.IsPrimary,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                };
                await _unitOfWork.Repository<Address>().AddAsync(address);
                await _unitOfWork.SaveChangesAsync();



                await transaction.CommitAsync();
                return address.AddressID;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Address creation failed: " + ex.Message);
            }
        }
        throw new NotImplementedException();
    }


}

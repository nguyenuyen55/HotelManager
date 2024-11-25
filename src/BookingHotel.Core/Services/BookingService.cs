using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.Models;
using BookingHotel.Core.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace BookingBooking.Api.Services
{
    public class BookingService
  {
    private readonly IBookingGenericRepository _bookingGenericRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BookingService> _logger;

    public BookingService(
        IBookingGenericRepository bookingGenericRepository,
        IUnitOfWork unitOfWork,
        ILogger<BookingService> logger)
    {
      _bookingGenericRepository = bookingGenericRepository;
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public async Task<ResponseData<IEnumerable<BookingResponseDto>>> GetAllBookingsAsync(string keyword)
    {
      var bookings = await _bookingGenericRepository.GetAllAsync(b => b.isActive); // Lấy tất cả booking đang hoạt động
      var contacts = await _unitOfWork.Repository<Contact>().GetAllAsync(); // Await the contacts
      var rooms = await _unitOfWork.Repository<Room>().GetAllAsync(); // Await the rooms
      var roomDetails = await _unitOfWork.Repository<RoomDetail>().GetAllAsync(); // Await the room details
      var persons = await _unitOfWork.Repository<Person>().GetAllAsync();
      var hotels = await _unitOfWork.Repository<Hotel>().GetAllAsync();

      var bookingDetails = from booking in bookings
                         join contact in contacts on booking.ContactID equals contact.Id into contactGroup
                         from contact in contactGroup.DefaultIfEmpty() // Left join
                         join p in persons on booking.UserID equals p.PersonID into personGroup
                         from person in personGroup.DefaultIfEmpty()
                         join room in rooms on booking.RoomID equals room.RoomID into roomGroup
                         from room in roomGroup.DefaultIfEmpty() // Left join
                         join roomDetail in roomDetails on room.RoomDetailID equals roomDetail.RoomDetailID into roomDetailGroup
                         from roomDetail in roomDetailGroup.DefaultIfEmpty() // Left join
                         join hotel in hotels on room.HotelID equals hotel.HotelID into hotelGroup
                         from hotel in hotelGroup.DefaultIfEmpty()
                         select new BookingResponseDto
                         {
                             BookingID = booking.BookingID,
                             FromDate = booking.FromDate,
                             ToDate = booking.ToDate,
                             BookingStatus = booking.BookingStatus,
                             CustomerName = contact != null ? contact.FullName : person.LastName + person.FirstName, // Lấy tên khách hàng
                             RoomType = roomDetail.RoomType,// Lấy loại phòng
                             HotelName = hotel.HotelName
                         };


      return new ResponseData<IEnumerable<BookingResponseDto>>(200, bookingDetails, "Success");
    }

    public async Task<ResponseData<Booking>> GetBookingByIdAsync(int id)
{
    // Tìm booking với ID được cung cấp
    var existingBooking = await _bookingGenericRepository.GetByIdAsync(id);

    if (existingBooking == null)
        return new ResponseData<Booking>(404, null, $"Booking with ID {id} not found.");

    // Trả về booking tìm thấy
    return new ResponseData<Booking>(200, existingBooking, "Success");
}
    public async Task<ResponseData<Booking>> InsertBookingAsync(Booking_InsertRequestData requestData)
    {
      //Kiểm tra nếu cả ContactID và UserID null thì báo lỗi
      var contactExists = await _unitOfWork.Repository<Contact>().GetByIdAsync(requestData.ContactID.Value);
      var userExists = await _unitOfWork.Repository<User>().GetByIdAsync(requestData.UserID.Value);

      if (contactExists == null && userExists == null)
      {
        return new ResponseData<Booking>(400, null, "Missing client information.");
      }

      var newBooking = new Booking
      {
        CreatedDate = DateTime.UtcNow.AddHours(7),
        RoomID = requestData.RoomID,
        ContactID = requestData.ContactID,
        UserID = requestData.UserID,
        DepositID = requestData.DepositID,
        FromDate = requestData.FromDate,
        CheckInDate = requestData.CheckInDate,
        CheckOutDate = requestData.CheckOutDate,
        BookingStatus = requestData.BookingStatus,
        Note = requestData.Note,
        ToDate = requestData.ToDate // Khách sạn mới luôn active
      };

      await _unitOfWork.Repository<Booking>().AddAsync(newBooking);
      await _unitOfWork.SaveChangesAsync();

      return new ResponseData<Booking>(201, newBooking, "Booking added successfully.");
    }
    public async Task<ResponseData<string>> UpdateBookingAsync(int id, Booking_InsertRequestData requestData)
    {

      // Tìm booking hiện có theo ID
      var existingBooking = await _bookingGenericRepository.GetByIdAsync(id);

      if (existingBooking == null)
        return new ResponseData<string>(404, null, $"Booking with ID {id} not found.");

       existingBooking.ToDate = requestData.ToDate;
       existingBooking.RoomID = requestData.RoomID;
       existingBooking.DepositID = requestData.DepositID;
       existingBooking.FromDate = requestData.FromDate;
       existingBooking.CheckInDate = requestData.CheckInDate;
       existingBooking.CheckOutDate = requestData.CheckOutDate;
       existingBooking.BookingStatus = requestData.BookingStatus;
            existingBooking.Note = requestData.Note;

      // Lưu thay đổi vào database
      await _unitOfWork.Repository<Booking>().UpdateAsync(existingBooking);
      await _unitOfWork.SaveChangesAsync();

      return new ResponseData<string>(200, null, "Booking updated successfully.");
    }


    public async Task<ResponseData<string>> DeleteBookingAsync(int id)
    {
      var existingBooking = await _bookingGenericRepository.GetByIdAsync(id);

      if (existingBooking == null || existingBooking.isActive == false)
        return new ResponseData<string>(404, null, $"Booking with ID {id} not found or already inactive.");

      existingBooking.isActive = false; // Thực hiện soft delete

      await _unitOfWork.Repository<Booking>().UpdateAsync(existingBooking);
      await _unitOfWork.SaveChangesAsync();

      return new ResponseData<string>(200, null, "Booking deleted successfully!!!.");
    }

    //public async Task<ResponseData<string>> UnIsActiveBookingAsync(int id)
    //{
    //  // Tìm khách sạn với ID được cung cấp
    //  var existingBooking = await _bookingGenericRepository.GetByIdAsync(id);

    //  if (existingBooking == null)
    //    return new ResponseData<string>(404, null, $"Booking with ID {id} not found.");

    //  // Kiểm tra nếu khách sạn đã active
    //  if (existingBooking.isActive == true)
    //    return new ResponseData<string>(400, null, "Booking is already active.");

    //  // Cập nhật trạng thái isActive thành true (khôi phục khách sạn)
    //  existingBooking.isActive = true;
    //  existingBooking.UpdatedDate = DateTime.UtcNow.AddHours(7);

    //  // Lưu thay đổi vào database
    //  await _unitOfWork.Repository<Booking>().UpdateAsync(existingBooking);
    //  await _unitOfWork.SaveChangesAsync();

    //  return new ResponseData<string>(200, null, "Booking is now active.");
    //}
  }
}

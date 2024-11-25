using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace BookingHotel.Api.Services
{
  public class HotelService
  {
    private readonly IHotelGenericRepository _hotelGenericRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HotelService> _logger;

    public HotelService(
        IHotelGenericRepository hotelGenericRepository,
        IUnitOfWork unitOfWork,
        ILogger<HotelService> logger)
    {
      _hotelGenericRepository = hotelGenericRepository;
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public async Task<ResponseData<IEnumerable<HotelResponseDto>>> GetAllHotelsAsync(string keyword)
    {
      var hotels = await _hotelGenericRepository.GetAllAsync(h => h.isActive == true);// Lọc theo isActive
      var addresses = await _unitOfWork.Repository<Address>().GetAllAsync();

      if (!string.IsNullOrWhiteSpace(keyword))
      {
        hotels = hotels
            .Where(h => h.HotelName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        h.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
      }
            var hotelList = from h in hotels
                            join a in addresses on h.AddressID equals a.AddressID into addressGroup
                            from ag in addressGroup.DefaultIfEmpty()
                            select new HotelResponseDto
                            {
                                HotelID = h.HotelID,
                                Address = ag.StreetAddress,
                                HotelName = h.HotelName,
                                Description = h.Description,
                                UrlImage = h.UrlImage,
                                CreatedDate = h.CreatedDate,
                                UpdatedDate = h.UpdatedDate,
                                isActive = h.isActive,
                                Rooms = h.Rooms,
                                Staffs = h.Staffs
                            };

      return new ResponseData<IEnumerable<HotelResponseDto>>(200, hotelList, "Success");
    }

    public async Task<ResponseData<Hotel>> GetHotelByIdAsync(int id)
{
    // Tìm khách sạn với ID được cung cấp
    var existingHotel = await _hotelGenericRepository.GetByIdAsync(id);

    if (existingHotel == null)
        return new ResponseData<Hotel>(404, null, $"Hotel with ID {id} not found.");

    // Trả về khách sạn tìm thấy
    return new ResponseData<Hotel>(200, existingHotel, "Success");
}
    public async Task<ResponseData<Hotel>> InsertHotelAsync(Hotel_InsertRequestData requestData)
    {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
      if (string.IsNullOrWhiteSpace(requestData.HotelName))
        return new ResponseData<Hotel>(400, null, "HotelName cannot be empty.");

      var hotelNameLower = requestData.HotelName.ToLower();

      // Lấy dữ liệu từ database và chuyển thành bộ nhớ để so sánh
      var existingHotel = await _hotelGenericRepository
          .GetAllAsync(h => h.HotelName.ToLower() == hotelNameLower);

      if (existingHotel.Any())
        return new ResponseData<Hotel>(400, null, "HotelName already exists.");

      // Xử lý ảnh nếu có
      string imageUrl = null;
      if (requestData.Image != null)
      {
        // Thư mục lưu ảnh sẽ là wwwroot/Images/Hotel
        var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotel");
        if (!Directory.Exists(imageDirectory))
        {
          Directory.CreateDirectory(imageDirectory);
        }

        // Tạo tên file duy nhất
        var fileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(requestData.Image.FileName)}";
        var filePath = Path.Combine(imageDirectory, fileName);

        // Lưu file ảnh vào thư mục wwwroot/Images/Hotel
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await requestData.Image.CopyToAsync(stream);
        }

        // Đường dẫn URL của ảnh
        imageUrl = $"/Images/Hotel/{fileName}";
      }

      var newHotel = new Hotel
      {
        CreatedDate = DateTime.UtcNow,
        HotelName = requestData.HotelName,
        Description = requestData.Description,
                        AddressID = requestData.AddressID,
        UrlImage = imageUrl, // Lưu đường dẫn ảnh
        isActive = true // Khách sạn mới luôn active
      };

      await _unitOfWork.Repository<Hotel>().AddAsync(newHotel);
      await _unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
      return new ResponseData<Hotel>(201, newHotel, "Hotel added successfully.");

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Address creation failed: " + ex.Message);
                }
                

            }
                
    }
    public async Task<ResponseData<string>> UpdateHotelAsync(int id, Hotel_InsertRequestData requestData)
    {
      if (string.IsNullOrWhiteSpace(requestData.HotelName))
        return new ResponseData<string>(400, null, "HotelName cannot be empty.");

      // Tìm khách sạn hiện có theo ID
      var existingHotel = await _hotelGenericRepository.GetByIdAsync(id);

      if (existingHotel == null)
        return new ResponseData<string>(404, null, $"Hotel with ID {id} not found.");

      var hotelNameLower = requestData.HotelName.ToLower();

      // Kiểm tra tên khách sạn trùng (không tính khách sạn hiện tại)
      var duplicateHotel = await _hotelGenericRepository
          .GetAllAsync(h => h.HotelName.ToLower() == hotelNameLower && h.HotelID != id);

      if (duplicateHotel.Any())
        return new ResponseData<string>(400, null, "HotelName already exists.");

      // Cập nhật tên và mô tả khách sạn
      existingHotel.HotelName = requestData.HotelName;
      existingHotel.Description = requestData.Description;
      existingHotel.UpdatedDate = DateTime.UtcNow.AddHours(7);
      // Xử lý ảnh nếu có
      if (requestData.Image != null)
      {
        // Thư mục lưu ảnh wwwroot/Images/Hotel
        var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotel");
        if (!Directory.Exists(imageDirectory))
        {
          Directory.CreateDirectory(imageDirectory);
        }

        // Tạo tên file duy nhất
        var fileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(requestData.Image.FileName)}";
        var filePath = Path.Combine(imageDirectory, fileName);

        // Lưu file ảnh vào thư mục wwwroot/Images/Hotel
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await requestData.Image.CopyToAsync(stream);
        }

        // Xóa ảnh cũ nếu có
        if (!string.IsNullOrWhiteSpace(existingHotel.UrlImage))
        {
          var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingHotel.UrlImage.TrimStart('/'));
          if (File.Exists(oldImagePath))
          {
            File.Delete(oldImagePath);
          }
        }

        // Cập nhật đường dẫn URL của ảnh mới
        existingHotel.UrlImage = $"/Images/Hotel/{fileName}";
      }

      // Lưu thay đổi vào database
      await _unitOfWork.Repository<Hotel>().UpdateAsync(existingHotel);
      await _unitOfWork.SaveChangesAsync();

      return new ResponseData<string>(200, null, "Hotel updated successfully.");
    }


    public async Task<ResponseData<string>> DeleteHotelAsync(int id)
    {
      var existingHotel = await _hotelGenericRepository.GetByIdAsync(id);

      if (existingHotel == null || existingHotel.isActive == false)
        return new ResponseData<string>(404, null, $"Hotel with ID {id} not found or already inactive.");

      existingHotel.isActive = false; // Thực hiện soft delete

      await _unitOfWork.Repository<Hotel>().UpdateAsync(existingHotel);
      await _unitOfWork.SaveChangesAsync();

      return new ResponseData<string>(200, null, "Hotel deleted successfully!!!.");
    }

    public async Task<ResponseData<string>> UnIsActiveHotelAsync(int id)
    {
      // Tìm khách sạn với ID được cung cấp
      var existingHotel = await _hotelGenericRepository.GetByIdAsync(id);

      if (existingHotel == null)
        return new ResponseData<string>(404, null, $"Hotel with ID {id} not found.");

      // Kiểm tra nếu khách sạn đã active
      if (existingHotel.isActive == true)
        return new ResponseData<string>(400, null, "Hotel is already active.");

      // Cập nhật trạng thái isActive thành true (khôi phục khách sạn)
      existingHotel.isActive = true;
      existingHotel.UpdatedDate = DateTime.UtcNow.AddHours(7);

      // Lưu thay đổi vào database
      await _unitOfWork.Repository<Hotel>().UpdateAsync(existingHotel);
      await _unitOfWork.SaveChangesAsync();

      return new ResponseData<string>(200, null, "Hotel is now active.");
    }
  }
}

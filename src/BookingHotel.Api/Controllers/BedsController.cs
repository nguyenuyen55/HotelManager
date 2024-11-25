using Azure;
using BookingHotel.Core;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public BedsController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork; 
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBed()
        {var beds=_unitOfWork.Repository<Bed>().GetAllAsync().Result;
            if(beds == null)
            {
                return NotFound("không có dữ liệu");
            }
            return Ok(beds);
        }
    }
}

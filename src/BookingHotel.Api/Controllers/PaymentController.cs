using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using BookingHotel.Core.Services;

namespace VNPAYPaymentBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("create-payment-url")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentRequest model)
        {
            if (model == null || model.Amount <= 0 || string.IsNullOrEmpty(model.TransactionId))
            {
                return BadRequest("Invalid payment request data.");
            }
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(new { Url = paymentUrl });
        }

         [HttpGet("vnpay_return")]
        public IActionResult VNPayReturn([FromQuery] Dictionary<string, string> vnpParams)
        {
            var result = _vnPayService.HandleReturnUrl(vnpParams);
       
            if (result.IsSuccess)
            {
                 _vnPayService.HandleIPN(vnpParams);
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
          [HttpPost("vnpay_ipn")]
        public IActionResult VNPayIPN([FromQuery] Dictionary<string, string> vnpParams)
        {
            var result = _vnPayService.HandleIPN(vnpParams);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookingHotel.Core.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentRequest model, HttpContext context);

        (bool IsSuccess, object Data, string ErrorMessage) HandleReturnUrl(Dictionary<string, string> vnpParams);
        
        (bool IsSuccess, object Data, string ErrorMessage) HandleIPN(Dictionary<string, string> vnpParams);
    }
}
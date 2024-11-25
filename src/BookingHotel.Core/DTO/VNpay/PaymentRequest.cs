public class PaymentRequest
{
  public decimal Amount { get; set; }                 // Số tiền
  public string TransactionId { get; set; }          // Mã giao dịch
  public string OrderDescription { get; set; }       // Nội dung thanh toán
  public string OrderType { get; set; }
}
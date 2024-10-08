using WebApi.Models;

namespace Data.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int? Order_id {  get; set; }
        public int? User_id {  get; set; }
        public DateTime Invoice_Date { get; set; }
        public double Total_Amount { get; set; }
        public string Status { get; set; }
        public virtual Order? Order { get; set; }
        public virtual User? User { get; set; }
    }
}

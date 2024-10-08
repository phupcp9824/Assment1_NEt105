using Data.Models;

namespace WebApi.Models
{
    public class Order
    {
        //  Lưu thông tin các đơn đặt hàng.
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? InvoiceId { get; set; }
        public DateTime DateOrder { get; set; }
        public string Status { get; set; }
        public double TotalPrice { get; set; }
        public string SippingAddress { get; set; }

        // 1 Order - N OrderDetai
        // // virtual giúp Entity framework có thể tải dữ liệu một cách hiệu quả
        public virtual List<OrderDetail>? OrderDetails { get; set; }
        public virtual User? User { get; set; }
        public virtual Invoice? Invoice { get; set; }


    }
}

namespace WebApi.Models
{
    public class OrderDetail
    {
        // lưu infor detail các món 
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? FastFoodItemId { get; set; }
        public int? ComboId { get; set; }
        public string? Usename { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Combo? Combo { get; set; }
        public virtual FastFoodItem? FastFoodItem { get; set; }

        // 1 orderdetail chỉ đc a C/F
    }
}

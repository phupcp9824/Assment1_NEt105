namespace WebApi.Models
{
    public class OrderDetail
    {
        // lưu infor detail các món 
        public int Id { get; set; }
        public int? IdOrder { get; set; }
        public int? IdFood { get; set; }
        public int? IdCombo { get; set; }
        public string? Usename { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Combo? Combo { get; set; }
        public virtual FastFoodItem? FastFoodItem { get; set; }

        // 1 orderdetail chỉ đc a C/F
    }
}

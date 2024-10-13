using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Combo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Range(0, 2000000)]
        public double Price { get; set; }

        [Range(1, 100)] 
        public int Quantity { get; set; }
        public string? size { get; set; }
        public string? Picture { get; set; } // lưu trữ đường dẫn hình ảnh trong csdl

        public string? Description { get; set; }

        public virtual List<OrderDetail>? OrderDetails { get; set; }

    }
}

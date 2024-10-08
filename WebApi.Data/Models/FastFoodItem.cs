using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class FastFoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Range(0, 10000)]
        public double Price { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
        public string? Picture { get; set; } // lưu trữ đường dẫn hình ảnh trong csdl

        public string? Description { get; set; }
 
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public virtual List<OrderDetail>? OrderDetails { get; set; }
    }
}

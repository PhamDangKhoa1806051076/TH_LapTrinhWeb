using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang2.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string PhoneNumber { get; set; }

        // Trạng thái đơn hàng: "Pending", "Approved", "Completed", "Cancelled"
        public string Status { get; set; } = "Pending";

        public List<OrderDetail> OrderDetails { get; set; }
    }
}

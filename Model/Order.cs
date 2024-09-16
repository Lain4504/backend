using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Model
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage = "The full name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The province is required")]
        public string Province { get; set; }

        [Required(ErrorMessage = "The district is required")]
        public string District { get; set; }

        [Required(ErrorMessage = "The ward is required")]
        public string Ward { get; set; }

        [Required(ErrorMessage = "The address is required")]
        public string Address { get; set; }

        public long? ShippingPrice { get; set; }

        [Required(ErrorMessage = "The phone is required")]
        [RegularExpression("^0\\d{9}$", ErrorMessage = "The phone is invalid")]
        public string Phone { get; set; }

        public string CustomerNote { get; set; }

        public string ShopNote { get; set; }

        public OrderState State { get; set; }

        public PaymentState PaymentState { get; set; }

        public ShippingState ShippingState { get; set; }

        public DateTime Created { get; set; }

        [Required(ErrorMessage = "The email is required")]
        public string Email { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public long GetTotalPrice()
        {
            long total = 0;
            if (OrderDetails != null && OrderDetails.Count > 0)
            {
                foreach (var detail in OrderDetails)
                {
                    total += detail.SalePrice * detail.Amount;
                }
                total += ShippingPrice.GetValueOrDefault();
            }
            return total;
        }
    }
    public enum OrderState
    {
        Pending,
        Processed,
        Shipped,
        Delivered,
        Canceled
    }
    public enum PaymentState
    {
        Unpaid,
        Paid,
        Refunded
    }
    public enum ShippingState
    {
        NotShipped,
        Shipped,
        Delivered
    }
   
}

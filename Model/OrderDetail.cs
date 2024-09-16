using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Model
{
    [Table("OrderDetail")]
    [Index(nameof(BookId), nameof(OrderId), IsUnique = true)]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Order")]
        public long OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Book")]
        public long BookId { get; set; }
        public virtual Book Book { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The number of units must be greater than 0")]
        public int Amount { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "The original price must be greater than 0")]
        public long OriginalPrice { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "The sale price must be greater than 0")]
        public long SalePrice { get; set; }

        public OrderDetailState GetOrderDetailState()
        {
            if (Amount > Book.Stock)
                return OrderDetailState.NotAvailable;
            return OrderDetailState.Available;
        }
    }
    public enum OrderDetailState
    {
        Available,
        NotAvailable
    }
}

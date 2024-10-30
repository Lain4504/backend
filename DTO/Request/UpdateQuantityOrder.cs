namespace BackEnd.DTO.Request
{
    public class UpdateQuantityOrder
    {
        public long orderId { get; set; }
        public long orderDetailId { set; get; }
        public int quantity { get; set; }
        public long SalePrice { get; set; }
    }
}

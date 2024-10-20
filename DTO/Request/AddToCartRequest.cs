namespace BackEnd.DTO.Request
{
    public class AddToCartRequest
    {
        public int BookId { get; set; }
        public decimal Price { get; set; } 
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }

}

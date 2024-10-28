namespace BackEnd.DTO.Response
{
    public class BestSellingProductDto
    {
        public long? BookId { get; set; }
        public string? Title { get; set; }
        public int TotalOrders { get; set; }
        public int TotalAmountSold { get; set; }
    }
}

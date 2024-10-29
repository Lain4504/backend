namespace BackEnd.DTO.Response
{
    public class TopSellingProductDto
    {
        public long BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public long TotalRevenue { get; set; }
    }
}

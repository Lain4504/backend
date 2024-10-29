namespace BackEnd.DTO.Response
{
    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public long TotalRevenue { get; set; }
    }
}

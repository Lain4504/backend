namespace BackEnd.DTO.Request
{
    public class UpdateOrderRequest
    {
        public long Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long TotalPrice { get; set; }
        public string Note { get; set; }
    }
}

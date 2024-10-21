namespace BackEnd.Models
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Revoked { get; set; } = false;
        public bool Used { get; set; } = false;

        public virtual User User { get; set; }
    }

}

namespace BackEnd.DTO.Request
{
    public class UserUpdateRequest
    {
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
    }
}

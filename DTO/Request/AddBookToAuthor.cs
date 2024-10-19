namespace BackEnd.DTO.Request
{
    public class AddBookToAuthorRequest
    {
        public long BookId { get; set; }

        public long AuthorId { get; set; }
    }
}

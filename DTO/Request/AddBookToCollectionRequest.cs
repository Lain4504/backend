namespace BackEnd.DTO.Request
{
    public class AddBookToCollectionRequest
    {
        public long BookId { get; set; }
        public long CollectionId { get; set; }
    }
}

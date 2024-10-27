using BackEnd.DTO.Request;
using BackEnd.Models;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAuthors();
    Task<Author> GetAuthorById(long id);
    Task DeleteAuthor(long id);
    Task AddAuthor(Author author);
    Task UpdateAuthor(Author author);
    Task<bool> AddBookToAuthor(long bookId, long authorId);
    Task<bool> RemoveAuthorFromBook(long bookId, long authorId);
}

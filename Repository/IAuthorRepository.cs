using BackEnd.DTO.Request;
using BackEnd.Models;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync();
    Task<Author> GetAuthorByIdAsync(long id);
    Task DeleteAuthorAsync(long id);
    Task AddAuthorAsync(Author author);
    Task UpdateAuthorAsync(Author author);
    Task<bool> AddBookToAuthorAsync(long bookId, long authorId);
    Task<bool> RemoveAuthorFromBook(long bookId, long authorId);
}

using BackEnd.DTO.Request;
using BackEnd.Models;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync();
    Task<Author> GetAuthorByIdAsync(long id);
    //Task DeleteAuthorAsync(long id);
}

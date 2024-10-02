using BackEnd.DTO.Request;
using BackEnd.Models;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAuthors();
    Task<Author> GetAuthorById(long id);
    //Task DeleteAuthor(long id);
}

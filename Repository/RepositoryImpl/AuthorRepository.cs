using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

public class AuthorRepository : IAuthorRepository
{
    private readonly BookStoreContext _context;

    public AuthorRepository(BookStoreContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author> GetAuthorByIdAsync(long id)
    {
        return await _context.Authors.FindAsync(id);
    }
    //public async Task DeleteAuthorAsync(long id)
    //{
    //    var author = await _context.Authors.FindAsync(id);
    //    if (author != null)
    //    {
    //        _context.Authors.Remove(author);
    //        await _context.SaveChangesAsync();
    //    }
    //}
}

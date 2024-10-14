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
    public async Task DeleteAuthorAsync(long id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddAuthorAsync(Author author)
    {
        var existingAuthor = await _context.Authors
            .FirstOrDefaultAsync(w => w.Id == author.Id);

        if (existingAuthor == null)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAuthorAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> AddBookToAuthorAsync(long bookId, long authorId)
    {
        var book = await _context.Books.FindAsync(bookId);
        var author = await _context.Authors.FindAsync(authorId);

        if (book == null)
        {
            throw new ArgumentException($"Book with ID {bookId} does not exist.");
        }

        if (author == null)
        {
            throw new ArgumentException($"Author with ID {authorId} does not exist.");
        }

        var exists = await _context.AuthorBooks
            .AnyAsync(bc => bc.BookId == bookId && bc.AuthorId == authorId);

        if (exists)
        {
            return false;
        }

        var bookAuthor = new AuthorBook
        {
            BookId = bookId,
            AuthorId = authorId
        };

        _context.AuthorBooks.Add(bookAuthor);
        await _context.SaveChangesAsync();
        return true; 
    }


}

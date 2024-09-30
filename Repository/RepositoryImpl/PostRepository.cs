using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class PostRepository : IPostRepository
    {
        private readonly BookStoreContext _context;

        public PostRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostByIdAsync(long id)
        {
            var qr = from w in _context.Posts
                     where w.Id == id
                     select w;
            return await qr.ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<PaginatedList<Post>> GetAllPostAsync(int page, int size, string sortBy, bool isAscending)
        {
            // Tải dữ liệu liên quan sử dụng Include và ThenInclude
            var source = _context.Posts
                .Include(b => b.User)
                .AsQueryable();

            if (isAscending)
            {
                source = source.OrderBy(post => EF.Property<object>(post, sortBy));
            }
            else
            {
                source = source.OrderByDescending(post => EF.Property<object>(post, sortBy));
            }

            return await PaginatedList<Post>.CreateAsync(source, page, size);
        }

        public async Task AddPostAsync(Post post)
        {
            var existingPost = await _context.Posts
            .FirstOrDefaultAsync(w => w.Title == post.Title);

            if (existingPost == null)
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePostAsync(long id)
        {
            var Post = await _context.Posts.FindAsync(id);
            if (Post != null)
            {
                // Remove the Post
                _context.Posts.Remove(Post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

    }

}

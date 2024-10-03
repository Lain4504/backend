using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Util;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class PostCategoryRepository : IPostCategoryRepository
    {
        private readonly BookStoreContext _context;

        public PostCategoryRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<PostCategory> GetPostCategoryByIdAsync(long id)
        {
            return await _context.PostCategories.FindAsync(id);
        }

        public async Task<IEnumerable<PostCategory>> GetAllCategoryAsync()
        {
            return await _context.PostCategories.ToListAsync();
        }

        public async Task AddPostCategoryAsync(PostCategory postCategory)
        {
            var existingPost = await _context.PostCategories
            .FirstOrDefaultAsync(w => w.Id == postCategory.Id);

            if (existingPost == null)
            {
                _context.PostCategories.Add(postCategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePostCategoryAsync(long id)
        {
            var postCategory = await _context.PostCategories.FirstOrDefaultAsync(b => b.Id == id);
            if (postCategory != null)
            {
                var post = await _context.Posts
                    .Where(ab => ab.CategoryId == id)
                    .ToListAsync();
                _context.Posts.RemoveRange(post);
                _context.PostCategories.Remove(postCategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePostCategoryAsync(PostCategory postCategory)
        {
            _context.PostCategories.Update(postCategory);
            await _context.SaveChangesAsync();
        }
    }

}

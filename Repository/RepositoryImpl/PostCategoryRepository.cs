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

        public async Task<List<PostCategory>> GetPostCategoryByIdAsync(long id)
        {
            var qr = from w in _context.PostCategories
                     where w.Id == id
                     select w;

            return await qr.ToListAsync();
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
            var postCategory = await _context.PostCategories.FindAsync(id);
            if (postCategory != null)
            {
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

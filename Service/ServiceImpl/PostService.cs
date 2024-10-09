using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Service.ServiceImpl
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        public PostService(IPostRepository repository)
        {
            _repository = repository;
        }

        public Task<Post> GetPostByIdAsync(long id)
        {
            return _repository.GetPostByIdAsync(id);
        }

        public async Task<IEnumerable<Post>> GetAllPostAsync()
        {
            return await _repository.GetAllAsync();
        }

        public Task<PaginatedList<Post>> GetAllPostAsync(int page, int size, string sortBy, bool isAscending)
        {
            return _repository.GetAllPostAsync(page, size, sortBy, isAscending);
        }
        public Task<bool> AddPostAsync(Post Post)
        {
            return _repository.AddPostAsync(Post);
        }

        public Task DeletePostAsync(long id)
        {
            return _repository.DeletePostAsync(id);
        }
         public Task UpdatePostAsync(Post post)
        {
            return _repository.UpdatePostAsync(post);
        }
        public async Task<IEnumerable<Post>> GetPostsByPostCategoryAsync(int? postcategoryId, string sortBy, string sortOrder)
        {
            var postsQuery = _repository.GetPosts();

            if (postcategoryId.HasValue)
            {
                postsQuery = _repository.GetPostsByPostCategory(postcategoryId.Value);
            }

            postsQuery = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? postsQuery.OrderBy(p => EF.Property<object>(p, sortBy))
                : postsQuery.OrderByDescending(p => EF.Property<object>(p, sortBy));

            return await postsQuery.ToListAsync();
        }

    }
}
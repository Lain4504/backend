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

        public Task<List<Post>> GetPostByIdAsync(long id)
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
        public Task AddPostAsync(Post Post)
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

    }
}
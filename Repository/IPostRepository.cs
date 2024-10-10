using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Repository
{
    public interface IPostRepository
    {
        Task<Post> GetPostByIdAsync(long id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<PaginatedList<Post>> GetAllPostAsync(int pageIndex, int pageSize, string sortBy, bool isAscending);
        Task<bool> AddPostAsync(Post Post);
        Task DeletePostAsync(long id);
        Task UpdatePostAsync(Post post);
        IQueryable<Post> GetPostsByPostCategory(int postcategoryId);
        IQueryable<Post> GetPosts();
    }
}

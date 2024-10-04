using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Service
{
    public interface IPostService
    {

        Task<Post> GetPostByIdAsync(long id);
        Task<IEnumerable<Post>> GetAllPostAsync();
        Task<PaginatedList<Post>> GetAllPostAsync(int page, int size, string sortBy, bool isAscending);
        Task<int> AddPostAsync(Post post);
        Task DeletePostAsync(long id);
        Task UpdatePostAsync(Post post);
    }
}
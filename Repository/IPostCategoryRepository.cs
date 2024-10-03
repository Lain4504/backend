using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Repository
{
    public interface IPostCategoryRepository
    {
        Task<PostCategory> GetPostCategoryByIdAsync(long id);
        Task<IEnumerable<PostCategory>> GetAllCategoryAsync();
        Task AddPostCategoryAsync(PostCategory Post);
        Task DeletePostCategoryAsync(long id);
        Task UpdatePostCategoryAsync(PostCategory post);
    }
}

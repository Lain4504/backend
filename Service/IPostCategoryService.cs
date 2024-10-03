using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Service
{
    public interface IPostCategoryService
    {

        Task<PostCategory> GetPostCategoryByIdAsync(long id);
        Task<IEnumerable<PostCategory>> GetAllPostCategoriesAsync();
        Task AddPostCategoryAsync(PostCategory post);
        Task DeletePostCategoryAsync(long id);
        Task UpdatePostCategoryAsync(PostCategory post);
    }
}
using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Service.ServiceImpl
{
    public class PostCategoryService : IPostCategoryService
    {
        private readonly IPostCategoryRepository _repository;
        public PostCategoryService(IPostCategoryRepository repository)
        {
            _repository = repository;
        }

        public Task<PostCategory> GetPostCategoryByIdAsync(long id)
        {
            return _repository.GetPostCategoryByIdAsync(id);
        }

        public async Task<IEnumerable<PostCategory>> GetAllPostCategoriesAsync()
        {
            return await _repository.GetAllCategoryAsync();
        }
        public Task AddPostCategoryAsync(PostCategory postCategory)
        {
            return _repository.AddPostCategoryAsync(postCategory);
        }

        public Task DeletePostCategoryAsync(long id)
        {
            return _repository.DeletePostCategoryAsync(id);
        }
         public Task UpdatePostCategoryAsync(PostCategory postCategory)
        {
            return _repository.UpdatePostCategoryAsync(postCategory);
        }

    }
}
//using BackEnd.Model;
//using BackEnd.Repository;
//using Microsoft.EntityFrameworkCore;

//namespace BackEnd.Service
//{
//    public class CollectionService : ICollectionService
//    {
//        private readonly ICollectionRepository _repository;
//        public CollectionService(ICollectionRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<bool> DeleteCollectionAsync(int id)
//        {
//            var collection = await _repository.GetCollectionByIdAsync(id);
//            if (collection == null)
//            {
//                return false; // Collection not found
//            }

//            await _repository.DeleteCollectionAsync(id);
//            return true; // Deletion successful
//        }



//        public async Task<List<Collection>> GetAllCollectionsAsync()
//        {
//            return await _repository.GetAllCollectionsAsync();
//        }

//        public async Task<Collection> GetCollectionByIdAsync(int id)
//        {
//            return await _repository.GetCollectionByIdAsync(id);
//        }

//        public async Task<Collection> SaveCollectionAsync(Collection collection)
//        {
//            return await _repository.SaveCollectionAsync(collection);   
//        }

//        public Task<Collection> UpdateCollectionAsync(Collection collection)
//        {
//            return _repository.UpdateCollectionAsync(collection);
//        }
//    }
//}

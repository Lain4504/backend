using BackEnd.Model;

namespace BackEnd.Repository
{
    public class ImageRepository : IImageRepository
    {
        public Task DeleteImagesAsync(IEnumerable<Image> images)
        {
            throw new NotImplementedException();
        }

        public Task SaveImageAsync(Image image)
        {
            throw new NotImplementedException();
        }
    }
}

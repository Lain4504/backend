using BackEnd.Model;

namespace BackEnd.Repository
{
    public interface IImageRepository
    {
        
            Task DeleteImagesAsync(IEnumerable<Image> images);
            Task SaveImageAsync(Image image);
    }
}

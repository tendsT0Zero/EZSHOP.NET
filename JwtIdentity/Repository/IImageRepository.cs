using EzShop.Api.Models;

namespace EzShop.Api.Repository
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}

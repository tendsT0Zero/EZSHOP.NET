using EzShop.Api.Data;
using EzShop.Api.Models;

namespace EzShop.Api.Repository
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDbContext context;

        public LocalImageRepository(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context)
        {
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public async Task<Image> Upload(Image image)
        {
            
            var uploadFolder = Path.Combine(environment.WebRootPath, "images");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

          
            var fileName = $"{image.FileName}{image.FileExtension}";
            var localFilePath = Path.Combine(uploadFolder, fileName);

            
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //  http://localhost:5000/images/xxx.jpg
            var url = $"{httpContextAccessor.HttpContext.Request.Scheme}://" +
                      $"{httpContextAccessor.HttpContext.Request.Host}" +
                      $"/images/{fileName}";

            image.FilePath = url;

        
            await context.Images.AddAsync(image);
            await context.SaveChangesAsync();

            return image;
        }
    }
}

using EzShop.Api.DTOs;
using EzShop.Api.Models;
using EzShop.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using Image = EzShop.Api.Models.Image;

namespace EzShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController:ControllerBase
    {
        private readonly IImageRepository imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        private void ValidateFileUpload(ImageUploadDTO image)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(Path.GetExtension(image.File.FileName)))
            {
                ModelState.AddModelError("File", "Unsupported file type. Allowed types are: " + string.Join(", ", allowedExtensions));

            }
            if(image.File.Length > 10485760)
            {
                ModelState.AddModelError("File", "File size exceeds the 10MB limit.");
            }
        }
        [Authorize(Roles = "Writer")]
        [HttpPost("updload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDTO request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                Image imageDomainModel = new Image
                {
                    File = request.File,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                    FileExtension= Path.GetExtension(request.File.FileName),
                    FileSizeInBytes= request.File.Length,

                };
                var uploadedImage = await imageRepository.Upload(imageDomainModel);
                return Ok(uploadedImage);

            }
            return BadRequest(ModelState);
        }
    }
}

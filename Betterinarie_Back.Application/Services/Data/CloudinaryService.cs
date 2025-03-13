using Betterinarie_Back.Core.Interfaces.Data;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Betterinarie_Back.Application.Services.Data
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinaryUrl = configuration["Cloudinary:URL"];
            _cloudinary = new Cloudinary(cloudinaryUrl);
            _cloudinary.Api.Secure = true;  
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile file, string folder, string fileName = null)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder,
                PublicId = fileName != null ? $"{folder}/{fileName}" : null,
                UseFilename = fileName == null,
                Overwrite = true
            };
            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult>DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}

using Betterinarie_Back.Core.Interfaces.Data;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

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

        public async Task<ImageUploadResult> UploadImage(Stream stream, string folder, string fileName = null)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName ?? Guid.NewGuid().ToString(), stream),
                Folder = folder,
                PublicId = fileName != null ? $"{folder}/{fileName}" : null,
                Overwrite = true
            };
            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
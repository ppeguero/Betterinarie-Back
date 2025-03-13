using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Betterinarie_Back.Core.Interfaces.Data
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImage(IFormFile file, string folder, string fileName = null);
        Task<DeletionResult> DeleteImage(string publicId);
    }
}

using CloudinaryDotNet.Actions;
using System.IO;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Interfaces.Data
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImage(Stream stream, string folder, string fileName = null);
        Task<DeletionResult> DeleteImage(string publicId);
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IBlobHelper
    {
        // Imagens envias por formulário
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);

        // Imagens enviadas por mobile
        Task<Guid> UploadBlobAsync(byte[] file, string containerName);

        // HTTP
        Task<Guid> UploadBlobAsync(string file, string containerName);
    }
}

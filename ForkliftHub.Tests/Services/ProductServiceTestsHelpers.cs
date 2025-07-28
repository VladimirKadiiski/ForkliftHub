using Microsoft.AspNetCore.Http;

namespace ForkliftHub.Tests.Services
{
    internal static class ProductServiceTestsHelpers
    {

        private static IFormFile MockFormFile(string fileName = "test.jpg")
        {
            var bytes = new byte[] { 1, 2, 3, 4 };
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "Images", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }
    }
}
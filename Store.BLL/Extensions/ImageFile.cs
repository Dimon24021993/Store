using System;
using System.IO;
using System.Threading.Tasks;

namespace Store.BLL.Extensions
{
    public static class ImageFile
    {
        public static async Task<string> UploadUserImageAsync(string fileName, string rootPath, Guid id, byte[] imageBytes)
        {
            //Image image = Image.FromStream(new MemoryStream(imageBytes));
            //todo add cut method to configurable size
            return await UploadImageAsync(fileName, rootPath, id, imageBytes);
        }

        public static async Task<string> UploadImageAsync(string fileName, string rootPath, Guid id, byte[] imageBytes)
        {
            var extension = Path.GetExtension(fileName).ToLower();

            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".jpe")
            {
                throw new Exception("Неверный формат файла");
            }

            Directory.CreateDirectory(rootPath);

            var filePath = rootPath + id + extension;

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
            }

            return id + extension;
        }
    }
}

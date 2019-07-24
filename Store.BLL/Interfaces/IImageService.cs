using System.Threading.Tasks;

namespace Store.BLL.Interfaces
{
    public interface IImageService
    {
        Task UploadImage(string path);
    }
}
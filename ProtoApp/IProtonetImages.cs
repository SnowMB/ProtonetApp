using System.Threading.Tasks;
using Windows.Storage;

namespace ProtoApp
{
    public interface IProtonetImages
    {
        string Server { get; set; }

        Task<StorageFolder> CreateFolder();
        Task<StorageFolder> GetFolder();
        Task<StorageFile> GetOrDownloadFullFile(string url, int id, string fileType);
        Task<StorageFile> GetOrDownloadThumbnailFile(string url, int id, string fileType);
    }
}
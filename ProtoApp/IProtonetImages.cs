using System.Threading.Tasks;
using Windows.Storage;

namespace ProtoApp
{
    public interface IProtonetImages
    {
        string Server { get; set; }

        Task<StorageFolder> CreateFolder();
        Task<StorageFile> DownLoadFile(string url, int id, string fileType);
        Task<StorageFolder> GetFolder();
        Task<StorageFile> GetImageLocal(int fileID, string fileType);
        Task<StorageFile> GetLocalOrDownloadImage(string url, int id, string fileType);
    }
}
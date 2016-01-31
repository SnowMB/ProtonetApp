using System.Threading.Tasks;
using Windows.Storage;

namespace ProtoApp
{
    public interface IProtonetImages
    {
        Task<StorageFile> DownLoadFile(string url, int id);
        Task<StorageFile> GetImageLocal(int fileID);
        Task<StorageFile> GetLocalOrDownloadImage(string url, int id);
    }
}
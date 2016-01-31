using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ProtoApp
{
    public class ProtonetImages : IProtonetImages
    {
        public string Server { get; set; } = null;
        public const string APP_NAME = "Protonet";


        private IProtonetClient service;

        public ProtonetImages(IProtonetClient dataService)
        {
            service = dataService;
        }




        public async Task<StorageFolder> GetFolder()
        {
            
            var folder = await KnownFolders.PicturesLibrary.TryGetItemAsync(APP_NAME) as StorageFolder;

            if (folder == null)
                return null;
            
            if (Server != null)
                folder = await folder?.GetFolderAsync(Server);

            return folder;
        }

        public async Task<StorageFolder> CreateFolder()
        {
            var appFolder = await KnownFolders.PicturesLibrary.TryGetItemAsync(APP_NAME) as StorageFolder;

            if (appFolder == null)
                appFolder = await KnownFolders.PicturesLibrary.CreateFolderAsync(APP_NAME);

            if (Server != null)
            {
                var serverFolder = await appFolder.TryGetItemAsync(Server) as StorageFolder;
                if (serverFolder == null)
                    appFolder = await appFolder.CreateFolderAsync(Server);
                else
                    appFolder = serverFolder;
            }

            return appFolder;

        }
        
        public async Task<StorageFile> GetLocalOrDownloadImage(string url, int id)
        {
            var file = await GetImageLocal(id);

            if (file == null)
                file = await DownLoadFile(url, id);

            return file;
        }

        public async Task<StorageFile> GetImageLocal(int fileID)
        {
            var folder = await GetFolder();
            var name = fileID.ToString();

            if (folder == null)
                return null;

            return await folder.TryGetItemAsync(name) as StorageFile;   

            
        }

        public async Task<StorageFile> DownLoadFile(string url, int id)
        {
            var folder = await GetFolder();
            if (folder == null)
                folder = await CreateFolder();


            var dl = await service.GetDownloadStream(url);

            var name = id.ToString();
            var file = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);

            var write = await file.OpenStreamForWriteAsync();
            dl.CopyTo(write);


            return file;
        }
    }
}

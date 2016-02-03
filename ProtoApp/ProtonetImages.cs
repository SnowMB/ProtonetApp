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
        

        public async Task<StorageFile> GetOrDownloadFullFile(string url, int id, string fileType)
        {
            var name = ConstructFileName(id, fileType);
            return await GetLocalOrDownloadImage(url, name);
        }

        public async Task<StorageFile> GetOrDownloadThumbnailFile(string url, int id, string fileType)
        {
            var name = ConstructThumbnailName(id, fileType);
            return await GetLocalOrDownloadImage(url, name);
        }


        private async Task<StorageFile> GetLocalOrDownloadImage(string url, string fileName)
        {
            var file = await GetImageLocal(fileName);
            
            if (file == null)
                file = await DownLoadFile(url, fileName);

            return file;
        }

        private async Task<StorageFile> GetImageLocal(string filename)
        {
            var folder = await GetFolder();
            
            if (folder == null)
                return null;

            return await folder.TryGetItemAsync(filename) as StorageFile;   

            
        }

        private async Task<StorageFile> DownLoadFile( string url, string fileName )
        {
            var folder = await GetFolder();
            if (folder == null)
                folder = await CreateFolder();

            Stream dl=null, write=null;

            try
            {
                dl = await service.GetDownloadStream(url);

                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                write = await file.OpenStreamForWriteAsync();
                dl.CopyTo(write);
                return file;
            }
            finally
            {
                write?.Dispose();
                dl?.Dispose();
            }
        }


        private string ConstructFileName(int id, string fileType) => $"{id}{ConstructFileEnding(fileType)}";
        
        private string ConstructThumbnailName(int id, string fileType) => $"{id}_thumb{ConstructFileEnding(fileType)}";

        private string ConstructFileEnding(string fileType)
        {
            switch(fileType)
            {
                case "image/jpeg": return ".jpg";
                default: return "";
            }
        }

    }
}

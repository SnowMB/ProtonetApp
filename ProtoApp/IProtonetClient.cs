using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ProtoApp.Objects;
using Windows.Storage;
using System.ComponentModel;

namespace ProtoApp
{
    public interface IProtonetClient : INotifyPropertyChanged
    {
        bool IsAuthentificated { get; }
        Me User { get; }

        event EventHandler AuthentificationComplete;
        event EventHandler AuthentificationFailed;
        event EventHandler LoggedOut;

        Task<bool> Authentificate(string tokenString);
        Task<bool> Authentificate(string user, string password);
        void CancelAllRequests();
        void Logout();
        Task<Meep> CreateMeep(string url, NewMeep meep);
        Task DownloadToFile(string url, StorageFile file);
        Task<PrivateChat> GetChat(string url);
        Task<List<Meep>> GetChatMeeps(string url);
        Task<List<PrivateChat>> GetChats();
        Task<Stream> GetDownloadStream(string url);
        Task<Me> GetMe();
        Task<TokenResponse> GetToken(string user, string password);
        void OnAuthentificationComplete();
        void OnAuthentificationFailed(Exception ex);
        void OnLoggedOut();
        Task<T> ReadGetResponseObjectFromUrl<T>(string url);
        Task<T> ReadPostResponseObjectFromUrl<T>(string url, HttpContent content);
    }
}
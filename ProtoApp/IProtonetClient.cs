using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ProtoApp.Objects;
using System.ComponentModel;

namespace ProtoApp
{
    public interface IProtonetClient : INotifyPropertyChanged
    {
        bool IsAuthentificated { get; }
        string Token { get; }
        Me User { get; }

        event EventHandler AuthentificationComplete;
        event EventHandler AuthentificationFailed;
        event EventHandler LoggedOut;


        void CancelAllRequests();
        void Logout();

        Task<bool> AuthentificateAsync(string tokenString);
        Task<bool> AuthentificateAsync(string user, string password);
        

        Task<Meep> CreateFileMeepAsync(string url, Stream file);
        Task<Meep> CreateMeepAsync(string url, NewMeep meep);

        //Task<TokenResponse> GetTokenAsync(string user, string password);

        Task<Me> GetMeAsync();
        Task<List<User>> GetUsersAsync(string url);
        Task<User> GetUserAsync(string url);

        Task<List<PrivateChat>> GetChatsAsync(string url);
        Task<PrivateChat> GetChatAsync(string url);

        Task<List<Meep>> GetMeepsAsync(string url);
        Task<Meep> GetMeepAsync(string url);

        Task<Stream> GetDownloadStreamAsync(string url);
        
        
    }
}
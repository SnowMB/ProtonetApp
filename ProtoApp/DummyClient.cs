using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ProtoApp.Objects;
using Windows.Storage;

namespace ProtoApp
{
    public class DummyClient : IProtonetClient
    {
        public bool IsAuthentificated => true;

        public string Token => "jkJh7(/6H/BgjkhlgGF/(43789KJgh";

        public Me User
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler AuthentificationComplete;
        public event EventHandler AuthentificationFailed;
        public event EventHandler LoggedOut;
        public event PropertyChangedEventHandler PropertyChanged;

        public Task<bool> Authentificate(string tokenString)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Authentificate(string user, string password)
        {
            throw new NotImplementedException();
        }

        public void CancelAllRequests()
        {
            throw new NotImplementedException();
        }

        public Task<Meep> CreateFileMeep(string url, Stream file)
        {
            throw new NotImplementedException();
        }

        public Task<Meep> CreateMeep(string url, NewMeep meep)
        {
            throw new NotImplementedException();
        }

        public Task DownloadToFile(string url, StorageFile file)
        {
            throw new NotImplementedException();
        }

        public Task<PrivateChat> GetChat(string url)
        {
            throw new NotImplementedException();
        }

        public Task<List<Meep>> GetChatMeeps(string url)
        {
            throw new NotImplementedException();
        }

        public Task<List<PrivateChat>> GetChats()
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetDownloadStream(string url)
        {
            throw new NotImplementedException();
        }

        public Task<Me> GetMe()
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> GetToken(string user, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void OnAuthentificationComplete()
        {
            throw new NotImplementedException();
        }

        public void OnAuthentificationFailed(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void OnLoggedOut()
        {
            throw new NotImplementedException();
        }

        public Task<T> ReadGetResponseObjectFromUrl<T>(string url)
        {
            throw new NotImplementedException();
        }

        public Task<T> ReadPostResponseObjectFromUrl<T>(string url, HttpContent content)
        {
            throw new NotImplementedException();
        }
    }
}

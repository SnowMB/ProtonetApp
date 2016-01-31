using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ProtoApp
{
    public class ProtonetClient : ObservableObject, IProtonetClient
    {
        const string API = "api/v1/";
        const string TOKEN = "tokens/";
        const string ME = "me/";

        const string USER = "users/";
        const string MEEPS = "meeps/";


        public bool IsAuthentificated => token != null;


        private Me user;
        public Me User
        {
            get { return user; }
            set
            {
                if (Set(nameof(User), ref user, value))
                    RaisePropertyChanged(nameof(IsAuthentificated));
            }
        }
        


        private string token;

        private CancellationTokenSource cts = new CancellationTokenSource();
        public void CancelAllRequests()
        {
            cts.Cancel();
            cts.Dispose();
            cts = new CancellationTokenSource();
        }

        public event EventHandler AuthentificationComplete;
        public void OnAuthentificationComplete() => AuthentificationComplete?.Invoke(this, EventArgs.Empty);

        public event EventHandler AuthentificationFailed;
        public void OnAuthentificationFailed(Exception ex) => AuthentificationFailed?.Invoke(this, new ExceptionEventArgs() { Exception = ex });

        public event EventHandler LoggedOut;
        public void OnLoggedOut() => LoggedOut?.Invoke(this, EventArgs.Empty);


        public class ExceptionEventArgs : EventArgs
        {
            public Exception Exception { get; set; }
        }



        private HttpClient client = new HttpClient();

        public ProtonetClient(string url)
        {
            if (!url.EndsWith(API))
            {
                if (!url.EndsWith("/"))
                    url += "/";
                url += API;
            }

            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
      


        
        public async Task<bool> Authentificate (string tokenString)
        {
            ClearLoginData();

            token = tokenString;

            await CreateAuthentificatedClientSettings();
            

            return true;
        }

        

        public async Task<bool> Authentificate(string user, string password)
        {
            ClearLoginData();

            var tokenResp = await GetToken(user, password);

            if (tokenResp == null)
                return false;

            token = tokenResp.Token;

            await CreateAuthentificatedClientSettings();

            return true;
        }

        private async Task CreateAuthentificatedClientSettings()
        {
            client.DefaultRequestHeaders.Add("X-Protonet-Token", token);
            User = await GetMe();

            OnAuthentificationComplete();
        }


        private void ClearLoginData()
        {
            User = null;
            token = null;
            client.DefaultRequestHeaders.Clear();
        }

        public void Logout()
        {
            ClearLoginData();
            OnLoggedOut();
        }


        private async Task<T> HandleAuthentificationError<T> (Task<T> action)
        {
            try
            {
                return await action;
            }
            catch (HttpRequestException ex)
            {
                ClearLoginData();
                OnAuthentificationFailed(ex);
                return default(T);
            }
            catch (Exception ex)
            {
                throw;
            }
        }










        
        public async Task<Me> GetMe ()
        {
            var response = await HandleAuthentificationError(ReadGetResponseObjectFromUrl<MeContainer>(ME));
            return response.Me;
        }

        public async Task<TokenResponse> GetToken(string user, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, TOKEN);
            
            var cred = $"{user}:{password}";
            var crypt = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));
            request.Headers.Add("Authorization", crypt);

            return await HandleAuthentificationError(Task.Run(async () =>
           {
               var resp = await client.SendAsync(request);
               resp.EnsureSuccessStatusCode();

               var json = await resp.Content.ReadAsStringAsync();

               return JsonConvert.DeserializeObject<TokenResponse>(json);
           }));
        }


        public async Task<List<PrivateChat>> GetChats()
        {
            var responseObject = await HandleAuthentificationError(ReadGetResponseObjectFromUrl<PrivateChatsContainer>(User.PrivateChatsUrl));
            return responseObject.Chats;
        } 

        public async Task<PrivateChat> GetChat(string url)
        {
            var responseObject = await HandleAuthentificationError(ReadGetResponseObjectFromUrl<PrivateChatContainer>(url));
            return responseObject.Chat;
        }

        public async Task<List<Meep>> GetChatMeeps(string url)
        {
            var responseObject = await HandleAuthentificationError(ReadGetResponseObjectFromUrl<MeepsContainer>(url));
            return responseObject.Meeps;
        } 


        public async Task<Meep> CreateMeep (string url, NewMeep meep)
        {
            var json = JsonConvert.SerializeObject(meep);
            var data = new StringContent(json);
            var responseObject = await HandleAuthentificationError(ReadPostResponseObjectFromUrl<MeepContainer>(url, data));
            return responseObject.Meep;
        }


        
        public async Task DownloadToFile (string url, StorageFile file)
        {
            var readstream = await GetDownloadStream(url);
            var writestream = await file.OpenStreamForWriteAsync();

            readstream.CopyTo(writestream);
        }







        

        public async Task<Stream> GetDownloadStream(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var resp = await client.SendAsync(request);
            return await resp.Content.ReadAsStreamAsync();
            
        }





        private async Task<string> ReadGetResponseFromUrl(string url)
        {
            var resp = await client.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();

        }
        public async Task<T> ReadGetResponseObjectFromUrl<T>(string url)
        {
            var json = await ReadGetResponseFromUrl(url);
            return JsonConvert.DeserializeObject<T>(json);
        }
        

        private async Task<string> ReadPostResponseFromUrl(string url, HttpContent content)
        {
            var resp = await client.PostAsync(url, content);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }
        public async Task<T> ReadPostResponseObjectFromUrl<T>(string url, HttpContent content )
        {
            content.Headers.ContentType.MediaType = "application/json";
            var json = await ReadPostResponseFromUrl(url, content);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

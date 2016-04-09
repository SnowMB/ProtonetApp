using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Windows.Storage;

namespace ProtoApp
{
    public class ProtonetClient : ObservableObject, IProtonetClient
    {
        const string API = "api/v1/";
        const string TOKEN = "tokens/";
        const string ME = "me/";

        const string USER = "users/";
        const string MEEPS = "meeps/";

        const string TOKEN_NAME = "X-Protonet-Token";


        public bool IsAuthentificated => Token != null;


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

        public string Server => User?.UserUrl.Replace( "users", "").Replace(API, "");
        


        public string Token { get; private set; }

        private CancellationTokenSource cts = new CancellationTokenSource();
        public void CancelAllRequests()
        {
            cts.Cancel();
            cts.Dispose();
            cts = new CancellationTokenSource();
        }

        public event EventHandler AuthentificationComplete;
        private void OnAuthentificationComplete() => AuthentificationComplete?.Invoke(this, EventArgs.Empty);

        public event EventHandler AuthentificationFailed;
        private void OnAuthentificationFailed() => AuthentificationFailed?.Invoke(this, EventArgs.Empty);

        public event EventHandler LoggedOut;
        private void OnLoggedOut() => LoggedOut?.Invoke(this, EventArgs.Empty);



        private HttpClient client = new HttpClient();

        public ProtonetClient()//string url)
        {
            //client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
      


        
        public async Task<bool> AuthentificateAsync (string server, string tokenString)
        {
            ClearLoginData();

            Token = tokenString;

            var meUrl = CreateValidMeUrl(server);
            await CreateAuthentificatedClientSettings(meUrl);
            

            return true;
        }

        

        public async Task<bool> AuthentificateAsync(string server, string user, string password)
        {
            ClearLoginData();

            var url = CreateValidTokenUrl(server);

            var tokenResp = await GetTokenAsync(url, user, password);

            if (string.IsNullOrWhiteSpace(tokenResp?.Token))
                return false;

            Token = tokenResp.Token;

            string meUrl = CreateValidMeUrl(server);
            await CreateAuthentificatedClientSettings(meUrl);

            return true;
        }

        private string CreateValidMeUrl(string url)
        {
            if (!url.EndsWith(ME))
            {
                if (!url.EndsWith(API))
                {
                    if (!url.EndsWith("/"))
                        url += "/";
                    url += API;
                }
        url += ME;
            }
            return url;
        }

        private static string CreateValidTokenUrl(string url)
        {
            if (!url.EndsWith(TOKEN))
            {
                if (!url.EndsWith(API))
                {
                    if (!url.EndsWith("/"))
                        url += "/";
                    url += API;
                }
        url += TOKEN;
            }

            return url;
        }

        private async Task CreateAuthentificatedClientSettings(string meUrl)
        {
            client.DefaultRequestHeaders.Add("X-Protonet-Token", Token);
            User = await GetMeAsync(meUrl);

            OnAuthentificationComplete();
        }


        private void ClearLoginData()
        {
            User = null;
            Token = null;
            client.DefaultRequestHeaders.Remove(TOKEN_NAME);
        }

        public void Logout()
        {
            ClearLoginData();
            OnLoggedOut();
        }


        public async Task<TokenResponse> GetTokenAsync(string url, string user, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            
            var cred = $"{user}:{password}";
            var crypt = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));
            request.Headers.Add("Authorization", crypt);

            return await SendAndReadResponseObject<TokenResponse>(request);
        }


        
        public async Task<Me> GetMeAsync (string url)
        {
            var response = await GetAndReadResponseObject<MeContainer>(url);
            
            return response?.Me;
        }

        

        public async Task<List<PrivateChat>> GetChatsAsync(string url)
        {
            var responseObject = await GetAndReadResponseObject<PrivateChatsContainer>(url);
            return responseObject?.Chats;
        } 

        public async Task<PrivateChat> GetChatAsync(string url)
        {
            var responseObject = await GetAndReadResponseObject<PrivateChatContainer>(url);
            return responseObject?.Chat;
        }

        public async Task<List<Meep>> GetMeepsAsync(string url)
        {
            var responseObject = await GetAndReadResponseObject<MeepsContainer>(url);
            return responseObject?.Meeps;
        } 

        public async Task<Meep> CreateMeepAsync (string url, MeepMessage meep)
        {
            var json = JsonConvert.SerializeObject(meep);
            var content = new StringContent(json);
            content.Headers.ContentType.MediaType = "application/json";
            var responseObject = await PostAndReadResponseObject<MeepContainer>(url, content);
            return responseObject.Meep;
        }

        public async Task<Meep> CreateFileMeepAsync (string url, Stream file)
        {
            var content = new StreamContent(file);
            //content.Headers.ContentType.MediaType = "application/octet-stream";
            var responseObject = await PostAndReadResponseObject<MeepContainer>(url, content);

            return responseObject.Meep;
        }


        

        public async Task<Stream> GetDownloadStreamAsync(string url)
        {
            return await GetAndReadResponseAsStream(url);
        }



        public Task<List<User>> GetUsersAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string url)
        {
            throw new NotImplementedException();
        }


        public Task<Meep> GetMeepAsync(string url)
        {
            throw new NotImplementedException();
        }







        public async Task CreatePushNotificationChannel(string url)
        {
            PushNotificationChannel channel = null;

            try
            {
                channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();


                var message = new DeviceMessage()
                {
                    UserID = User.ID,
                    Token = channel.Uri,
                    Platform = "windows",
                    Uuid = Guid.NewGuid(),
                    AppName = "ProtonetApp",
                    Model = "PC",
                    AppVersion = "0.1"
                };

                var content = new StringContent(JsonConvert.SerializeObject(message));
                content.Headers.ContentType.MediaType = "application/json";

                var dev = await PostAndReadResponseObject<Device>(User.DevicesUrl, content);

                channel.PushNotificationReceived += Channel_PushNotificationReceived;
            }

            catch //(Exception ex)
            {
                throw;
            }
        }

        private void Channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async Task<Stream> GetAndReadResponseAsStream(string url)
        {
            var response = await HandleUnauthorizedAccess(GetAsync(url));

            return await response.Content.ReadAsStreamAsync();
        }

        private async Task<HttpResponseMessage> GetAsync(string url)
        {
            var resp = await client.GetAsync(url);
            await CheckResponseStatus(resp);
            return resp;
        }

        
        private async Task<string> SendAndReadResponseAsString(HttpRequestMessage message)
        {
            var resp = await client.SendAsync(message);
            await CheckResponseStatus(resp);

            return await resp.Content.ReadAsStringAsync();
        }
        private async Task<T> SendAndReadResponseObject<T>(HttpRequestMessage message)
        {
            var json = await HandleUnauthorizedAccess(SendAndReadResponseAsString(message));

            return JsonConvert.DeserializeObject<T>(json);
        }

 
        private async Task<string> GetAndReadResponseAsString(string url)
        {
            var resp = await GetAsync(url);

            return await resp.Content.ReadAsStringAsync();

        }
        private async Task<T> GetAndReadResponseObject<T>(string url)
        {
            var json = await HandleUnauthorizedAccess(GetAndReadResponseAsString(url));

            return JsonConvert.DeserializeObject<T>(json);
        }
        

        private async Task<string> PostAndReadResponseAsString(string url, HttpContent content)
        {
            var resp = await client.PostAsync(url, content);

            await CheckResponseStatus(resp);

            return await resp.Content.ReadAsStringAsync();
        }
        private async Task<T> PostAndReadResponseObject<T>(string url, HttpContent content )
        {
            var json = await HandleUnauthorizedAccess(PostAndReadResponseAsString(url, content));
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task<T> HandleUnauthorizedAccess<T>(Task<T> task)
        {
            try
            {
                return await task;
            }
            catch (UnauthorizedAccessException)
            {
                ClearLoginData();
                OnAuthentificationFailed();
                return await Task.FromResult(default(T));
            }
        }

        private async Task CheckResponseStatus(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException();
                case HttpStatusCode.BadRequest:
                    Debug.WriteLine(await response.Content.ReadAsStringAsync()); break;
                //....
            }
            response.EnsureSuccessStatusCode();
        }

        
    }
}

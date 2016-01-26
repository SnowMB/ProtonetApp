using Newtonsoft.Json;
using ProtonetApp.Objects;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ProtoLib
{
    public class ProtoNet
    {
        const string TOKEN = "tokens";
        const string ME = "me";
        const string PROJECT = "projects";
        const string CHATS = "private_chats";
        const string USER = "users";

        private CancellationTokenSource cts = new CancellationTokenSource();
        private HttpClient client;


        public Uri URI { get; set; }
        public string Token { get; set; }



        public ProtoNet()
        {
            var filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);
            client = new HttpClient(filter);
        }
        public ProtoNet(string url)
        {
            URI = new Uri(url);
        }

        public void CancelAllRequests()
        {
            cts.Cancel();
            cts.Dispose();
            cts = new CancellationTokenSource();
        }

        public async Task<TokenResponse> getToken(string user, string password)
        {
            var json = await getTokenString(user, password);
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }
        public async Task<string> getTokenString(string user, string password)
        {
            var request = new HttpRequestMessage( HttpMethod.Get, new Uri(URI, TOKEN));

            var cred = $"{user}:{password}";
            var crypt  =Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));

            request.Headers.Add("Authorization", crypt);

            var response = await client.SendRequestAsync(request).AsTask(cts.Token);
            

            return await ReadResponse(response);
        }


        public async Task<Me> getMe()
        {
            var json = await getMeString();
            return JsonConvert.DeserializeObject<Me>(json);
        }
        public async Task<string> getMeString()
        {
            var request = CreateRequestWithToken(HttpMethod.Get, new Uri(URI, ME));

            return await SendRequestAndReadResponse(request);
        }

        
        public async Task<PrivateChats> getPrivateChats(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
        {
            var json = await getPrivateChatsString(excludeEmpty, offset, limit, other_user_id);
            return JsonConvert.DeserializeObject<PrivateChats>(json);
        }
        public async Task<string> getPrivateChatsString(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
        {
            var request = CreateRequestWithToken(HttpMethod.Get, new Uri(URI, CHATS));

            return await SendRequestAndReadResponse(request);
        }





        private HttpRequestMessage CreateRequestWithToken(HttpMethod method, Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, uri);
            request.Headers.Add("X-Protonet-Token", Token);
            return request;
        }
        private async Task<string> ReadResponse(HttpResponseMessage response) => await response.Content.ReadAsStringAsync().AsTask(cts.Token);
        private async Task<string> SendRequestAndReadResponse(HttpRequestMessage request) => await ReadResponse(await client.SendRequestAsync(request).AsTask());
    }
}

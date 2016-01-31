using Newtonsoft.Json;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;
using Windows.Storage.Streams;

namespace ProtoApp
{
    public class ProtonetDataService : IProtonetDataService
    {
        const string API = "api/v1/";
        const string TOKEN = "tokens/";
        const string ME = "me/";
        const string PROJECTs = "projects/";
        const string CHATS = "private_chats/";
        const string USER = "users/";
        const string TOPICS = "topics/";
        const string MEEPS = "meeps/";

        private CancellationTokenSource cts = new CancellationTokenSource();
        private HttpClient client;


        public Uri URI { get; set; }
        public string Token { get; set; }



        public ProtonetDataService()
        {
            var filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            client = new HttpClient(filter);
        }
        public ProtonetDataService(string url) : this()
        {
            if (!url.EndsWith(API))
            {
                if (!url.EndsWith("/"))
                    url += "/";
                url += API;
            }

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
            return ConvertToObject<TokenResponse>(json);
        }
        public async Task<string> getTokenString(string user, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, TokenUri);

            var cred = $"{user}:{password}";
            var crypt = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));

            request.Headers.Add("Authorization", crypt);

            return await SendRequestAndReadResponse(request);
        }


        public async Task<Me> getMe()
        {
            var json = await getMeString();
            return ConvertToObject<Me>(json);
        }
        public async Task<string> getMeString()
        {
            var request = CreateRequestWithToken(HttpMethod.Get, MeUri);

            return await SendRequestAndReadResponse(request);
        }


        public async Task<List<PrivateChat>> getPrivateChats(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
        {
            var json = await getPrivateChatsString(excludeEmpty, offset, limit, other_user_id);
            return ConvertToObject<PrivateChatsContainer>(json).Chats;
        }
        public async Task<string> getPrivateChatsString(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
        {
            var request = CreateRequestWithToken(HttpMethod.Get, ChatsUri);

            return await SendRequestAndReadResponse(request);
        }


        public async Task<PrivateChat> getPrivateChat(int id)
        {
            var json = await getPrivateChatString(id);
            return ConvertToObject<PrivateChatContainer>(json).Chat;
        }
        public async Task<string> getPrivateChatString(int id)
        {
            var uri = AddIdToUri(ChatsUri, id);
            var request = CreateRequestWithToken(HttpMethod.Get, uri);

            return await SendRequestAndReadResponse(request);
        }


        public async Task<List<Meep>> getMeeps(int id, ObjectType type)
        {
            var json = await getMeepsString(id, type);
            return ConvertToObject<MeepsContainer>(json).Meeps;
        }
        public async Task<string> getMeepsString(int id, ObjectType type)
        {
            var uri = GetMeepsUri(id, type);
            var request = CreateRequestWithToken(HttpMethod.Get, uri);

            return await SendRequestAndReadResponse(request);
        }
        

        public async Task<Meep> getMeep(int objectId, ObjectType type, int meepId)
        {
            var json = await getMeepString(objectId, type, meepId);
            return ConvertToObject<MeepContainer>(json).Meep;
        }
        public async Task<string> getMeepString(int objectId, ObjectType type, int meepId)
        {
            var uri = AddIdToUri(GetMeepsUri(objectId, type), meepId);
            var request = CreateRequestWithToken(HttpMethod.Get, uri);
            
            return await SendRequestAndReadResponse(request);
        }




        public async Task<Meep> createMeep(int objectId, ObjectType type, string message)
        {
            var json = await createMeepString(objectId, type, message);
            return ConvertToObject<MeepContainer>(json).Meep;
        }
        public async Task<string> createMeepString(int objectId, ObjectType type, string message)
        {
            var uri = GetMeepsUri(objectId, type);
            var request = CreateRequestWithToken(HttpMethod.Post, uri);
            request.Content = new HttpStringContent("{" + $"\"message\":\"{message}\"" + "}");
            request.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");

            return await SendRequestAndReadResponse(request);
        }


        public async Task<IBuffer> GetDownloadBuffer(string url)
        {
            var request = CreateRequestWithToken(HttpMethod.Get, new Uri(url));
            var response = await client.SendRequestAsync(request);
            return await response.Content.ReadAsBufferAsync();
        }



        private Uri TokenUri => new Uri(URI, TOKEN);
        private Uri MeUri => new Uri(URI, ME);
        private Uri TopicsUri => new Uri(URI, TOPICS);
        private Uri ProjectUri => new Uri(URI, PROJECTs);
        private Uri ChatsUri => new Uri(URI, CHATS);
       
        private Uri GetUriFromType(ObjectType type)
        {
            switch (type)
            {
                case ObjectType.PrivateChat: return ChatsUri;
                case ObjectType.Project: return ProjectUri;
                case ObjectType.Topic: return TopicsUri;
                default: throw new NotImplementedException($"Could not get Meeps for type {type}. Method is not implemented.");
            }
        }
        private Uri AddIdToUri(Uri uri, int id) => new Uri(uri, $"{id}/");
        private Uri GetMeepsUri(int id, ObjectType type) => new Uri(AddIdToUri(GetUriFromType(type), id), MEEPS);

        private HttpRequestMessage CreateRequestWithToken(HttpMethod method, Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, uri);
            request.Headers.Add("X-Protonet-Token", Token);
            return request;
        }
        private async Task<string> ReadResponse(HttpResponseMessage response) => await response.Content.ReadAsStringAsync().AsTask(cts.Token);
        private async Task<string> SendRequestAndReadResponse(HttpRequestMessage request) => await ReadResponse(await client.SendRequestAsync(request).AsTask());
        private T ConvertToObject<T> (string json)
        {
            Debug.WriteLine(json);
            var obj = JsonConvert.DeserializeObject<T>(json);
            Debug.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
            return obj;
        }

        
    }
}

//using Newtonsoft.Json;
//using ProtoApp.Objects;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Windows.Security.Cryptography.Certificates;
//using System.Net.Http;

//namespace ProtoApp
//{
//    public class ProtonetDataService : IProtonetDataService
//    {
//        private CancellationTokenSource cts = new CancellationTokenSource();
//        public HttpClient Client { get; set; }
        

//        public Uri URI { get; set; }
//        public string Token { get; set; }


//        public ProtonetDataService()
//        {
//            Client = new HttpClient();
//        }

//        public ProtonetDataService(HttpClient client)
//        {
//            Client = client;
//        }
        
//        public void CancelAllRequests()
//        {
//            cts.Cancel();
//            cts.Dispose();
//            cts = new CancellationTokenSource();
//        }

        
//        public async Task<TokenResponse> getTokenAsync(string url, string user, string password)
//        {
//            var json = await getTokenStringAsync(url, user, password);
//            return ConvertToObject<TokenResponse>(json);
//        }
//        public async Task<string> getTokenStringAsync(string url, string user, string password)
//        {
//            var request = new HttpRequestMessage(HttpMethod.Post, url);

//            var cred = $"{user}:{password}";
//            var crypt = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));

//            request.Headers.Add("Authorization", crypt);

//            return await GetResponseAsync(request);
//        }


//        public async Task<string> getMeString()
//        {
//            var request = CreateRequestWithToken(HttpMethod.Get, MeUri);

//            return await SendRequestAndReadResponse(request);
//        }


//        public async Task<List<PrivateChat>> getPrivateChats(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
//        {
//            var json = await getPrivateChatsString(excludeEmpty, offset, limit, other_user_id);
//            return ConvertToObject<PrivateChatsContainer>(json).Chats;
//        }
//        public async Task<string> getPrivateChatsString(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
//        {
//            var request = CreateRequestWithToken(HttpMethod.Get, ChatsUri);

//            return await SendRequestAndReadResponse(request);
//        }


//        public async Task<PrivateChat> getPrivateChat(int id)
//        {
//            var json = await getPrivateChatString(id);
//            return ConvertToObject<PrivateChatContainer>(json).Chat;
//        }
//        public async Task<string> getPrivateChatString(int id)
//        {
//            var uri = AddIdToUri(ChatsUri, id);
//            var request = CreateRequestWithToken(HttpMethod.Get, uri);

//            return await SendRequestAndReadResponse(request);
//        }


//        public async Task<List<Meep>> getMeeps(int id, ObjectType type)
//        {
//            var json = await getMeepsString(id, type);
//            return ConvertToObject<MeepsContainer>(json).Meeps;
//        }
//        public async Task<string> getMeepsString(int id, ObjectType type)
//        {
//            var uri = GetMeepsUri(id, type);
//            var request = CreateRequestWithToken(HttpMethod.Get, uri);

//            return await SendRequestAndReadResponse(request);
//        }
        

//        public async Task<Meep> getMeep(int objectId, ObjectType type, int meepId)
//        {
//            var json = await getMeepString(objectId, type, meepId);
//            return ConvertToObject<MeepContainer>(json).Meep;
//        }
//        public async Task<string> getMeepString(int objectId, ObjectType type, int meepId)
//        {
//            var uri = AddIdToUri(GetMeepsUri(objectId, type), meepId);
//            var request = CreateRequestWithToken(HttpMethod.Get, uri);
            
//            return await SendRequestAndReadResponse(request);
//        }




//        public async Task<Meep> createMeep(int objectId, ObjectType type, string message)
//        {
//            var json = await createMeepString(objectId, type, message);
//            return ConvertToObject<MeepContainer>(json).Meep;
//        }
//        public async Task<string> createMeepString(int objectId, ObjectType type, string message)
//        {
//            var uri = GetMeepsUri(objectId, type);
//            var request = CreateRequestWithToken(HttpMethod.Post, uri);
//            request.Content = new HttpStringContent("{" + $"\"message\":\"{message}\"" + "}");
//            request.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");

//            return await SendRequestAndReadResponse(request);
//        }



//        private async Task<string> GetResponseAsync(HttpRequestMessage message) => await (await Client.SendAsync(message, cts.Token)).Content.ReadAsStringAsync();


//        private HttpRequestMessage CreateRequestWithToken(HttpMethod method, Uri uri)
//        {
//            HttpRequestMessage request = new HttpRequestMessage(method, uri);
//            request.Headers.Add("X-Protonet-Token", Token);
//            return request;
//        }

        
        
//        private T ConvertToObject<T> (string json)
//        {
//            Debug.WriteLine(json);
//            var obj = JsonConvert.DeserializeObject<T>(json);
//            Debug.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
//            return obj;
//        }

        
//    }
//}

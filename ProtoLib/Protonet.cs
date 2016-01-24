using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProtoLib
{
    public class ProtoNet
    {
        const string TOKEN = "tokens";
        const string ME = "me";
        const string PROJECT = "projects";
        const string CHATS = "private_chats";
        const string USER = "users";

        public Uri URI { get; set; }
        public string Token { get; set; }

        public ProtoNet()
        {

        }

        public ProtoNet(string url)
        {
            URI = new Uri(url);
        }

        public TokenResponse getToken(string user, SecureString password)
        {
            var json = getTokenString(user, password);
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }

        public string getTokenString(string user, SecureString password)
        {
            //credentials.Add ( new System.Uri ( uri ), "Basic", new NetworkCredential ( boxUser.Text, boxPw.SecurePassword) );
            //var url = Path.Combine ( URL, TOKEN );

            var client = new HttpClient();
            var content = new HttpRequestMessage(HttpMethod.Post, new Uri(URI, TOKEN));
            content.Headers()
            client.PostAsync(new Uri(URI, TOKEN),);

            var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(URI, TOKEN));
            request.ContentType = "text/json";
            request.Credentials = new NetworkCredential(user, password);
            request.Method = "POST";

            //var objStream = request.GetRequestStream ();
            //var writer = new StreamWriter ( objStream );

            //writer.Write ( "{\"comment\":\"ProtonetTool\"}" );
            //writer.Flush ();

            //writer.Close ();

            return ReadResponse(request);
        }

        private string ReadResponse(WebRequest request)
        {
            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                using (var respReader = new StreamReader(responseStream))
                {
                    return respReader.ReadLine();
                }
            }
        }

        public string getMeString()
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(URI, ME));
            request.ContentType = "text/json";
            request.Method = "GET";

            request.Headers["X-Protonet-Token"] = Token;

            return ReadResponse(request);
        }


        public PrivateChats getPrivateChats(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
        {
            var json = getPrivateChatsString(excludeEmpty, offset, limit, other_user_id);
            return JsonConvert.DeserializeObject<PrivateChats>(json);
        }

        public string getPrivateChatsString(bool excludeEmpty = false, int? offset = null, int? limit = null, int? other_user_id = null)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(URI, CHATS));
            request.ContentType = "text/json";
            request.Method = "GET";

            request.Headers["X-Protonet-Token"] = Token;

            return ReadResponse(request);
        }
    }
}

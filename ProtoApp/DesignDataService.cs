using System;
using System.Threading.Tasks;
using ProtoApp.Objects;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;
using System.Collections.Generic;

namespace ProtoApp
{
    internal class DesignDataService : IProtonetDataService
    {
        public string Token { get; set; }

        public void CancelAllRequests()
        {
            throw new NotImplementedException();
        }

        public async Task<Me> getMe()
        {
            return new Me()
            {
                Avatar = "...",
                CreatedAt = "created at",
                Deactivated = false,
                Email = "test@test.test",
                LastActiveAt = "last active at",
                UpdatedAt = "updated at",
                DevicesUrl = "no url",
                FirstName = "firstname",
                LastName = "lastname",
                ID = 9,
                Online = false,
                PrivateChatsUrl = "no url",
                ProjectsUrl = "no url",
                Role = "User",
                Url = "no url",
                UserName = "testuser",
                UserUrl = "no url"
            };
        }
        public async Task<string> getMeString()
        {
            var obj = await getMe();
            return SerializeObject(obj); 
        }      

        public async Task<PrivateChats> getPrivateChats(bool excludeEmpty = false, int? offset = default(int?), int? limit = default(int?), int? other_user_id = default(int?))
        {
            return new PrivateChats()
            {
                Chats = new List<PrivateChat>()
                {
                    await getPrivateChat(8)
                }
            };
        }
        public async Task<string> getPrivateChatsString(bool excludeEmpty = false, int? offset = default(int?), int? limit = default(int?), int? other_user_id = default(int?))
        {
            var obj = await getPrivateChats(excludeEmpty,offset,limit,other_user_id);
            return SerializeObject(obj);
        }


        public async Task<PrivateChat> getPrivateChat(int id)
        {
            return new PrivateChat()
            {
                CreatedAt = "now",
                ID = id,
                CurrentMeepNumber = 0,
                LastMeep = new Meep(),
                LastMeepDate = "now",
                MeepsUrl = "no url",
                NotificationID = 9,
                NotificationsCount = 2,
                OtherUsers = new List<User>() { await getMe() },
                SubsciptionsURL = "no url",
                UpdatedAt = "yesterday",
                Url = "no url"
            };
        }
        public async Task<string> getPrivateChatString(int id)
        {
            var obj = await getPrivateChat(id);
            return SerializeObject(obj);
        }

        public async Task<TokenResponse> getToken(string user, string password)
        {
            if (user == "test" && password == "1234")
                return new TokenResponse()
                {
                    Comment = "Nur test",
                    CreatedAt = "now",
                    ID = 8,
                    OwnerID = 1,
                    UserID = 0,
                    OwnerType = "User",
                    Token = "jklÜdakjJKLBbKLBhBKLJBHsdfJd",
                    UpdatedAt = "never",
                    Url = "no url"
                };

            throw new Exception("Failed to log in!");
        }
        public async Task<string> getTokenString(string user, string password)
        {
            var obj = await getToken(user,password);
            return SerializeObject(obj);
        }

        public async Task<Meeps> getMeeps(int id, ObjectType type)
        {
            return new Meeps()
            {
                Messages = new List<Meep>()
                {
                    await getMeep(id, type, 12)
                }
            };
        }
        public async Task<string> getMeepsString(int id, ObjectType type)
        {
            var obj = await getPrivateChat(id);
            return SerializeObject(obj);
        }

        public async Task<Meep> getMeep(int objectId, ObjectType type, int meepId)
        {
            return new Meep()
            {
                CreatedAt = "now",
                ID = meepId,
                Message = "das ist eine Dummynachricht",
                Number = 0,
                Type = "Meep",
                UpdatedAt = "now",
                Url = "no url",
                User = await getMe()
            };
        }
        public async Task<string> getMeepString(int objectId, ObjectType type, int meepId)
        {
            var obj = await getMeep(objectId, type, meepId);
            return SerializeObject(obj);
        }
    }
}
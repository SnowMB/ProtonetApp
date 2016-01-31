using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProtoApp.Objects;
using Windows.Storage.Streams;

namespace ProtoApp
{
    public interface IProtonetDataService
    {
        string Token { get; set; }

        void CancelAllRequests();
        Task<Meep> createMeep(int objectId, ObjectType type, string message);
        Task<string> createMeepString(int objectId, ObjectType type, string message);
        Task<Me> getMe();
        Task<Meep> getMeep(int objectId, ObjectType type, int meepId);
        Task<List<Meep>> getMeeps(int id, ObjectType type);
        Task<string> getMeepsString(int id, ObjectType type);
        Task<string> getMeepString(int objectId, ObjectType type, int meepId);
        Task<string> getMeString();
        Task<PrivateChat> getPrivateChat(int id);
        Task<List<PrivateChat>> getPrivateChats(bool excludeEmpty = false, int? offset = default(int?), int? limit = default(int?), int? other_user_id = default(int?));
        Task<string> getPrivateChatsString(bool excludeEmpty = false, int? offset = default(int?), int? limit = default(int?), int? other_user_id = default(int?));
        Task<string> getPrivateChatString(int id);
        Task<TokenResponse> getToken(string user, string password);
        Task<IBuffer> GetDownloadBuffer(string url);
        Task<string> getTokenString(string user, string password);
    }

    public enum ObjectType
    {
        PrivateChat,
        Topic,
        Project
    }
}
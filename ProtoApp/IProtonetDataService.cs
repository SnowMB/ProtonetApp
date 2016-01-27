using System;
using System.Threading.Tasks;
using ProtoApp.Objects;

namespace ProtoApp
{
    public interface IProtonetDataService
    {
        string Token { get; set; }

        void CancelAllRequests();

        Task<Me> getMe();
        Task<string> getMeString();

        Task<PrivateChats> getPrivateChats(bool excludeEmpty = false, int? offset = default(int?), int? limit = default(int?), int? other_user_id = default(int?));
        Task<string> getPrivateChatsString(bool excludeEmpty = false, int? offset = default(int?), int? limit = default(int?), int? other_user_id = default(int?));

        Task<TokenResponse> getToken(string user, string password);
        Task<string> getTokenString(string user, string password);

        Task<PrivateChat> getPrivateChat(int id);
        Task<string> getPrivateChatString(int id);

        Task<Meeps> getMeeps(int id, ObjectType type);
        Task<string> getMeepsString(int id, ObjectType type);

        Task<Meep> getMeep(int objectId, ObjectType type, int meepId);
        Task<string> getMeepString(int objectId, ObjectType type, int meepId);
    }

    public enum ObjectType
    {
        Topic,
        Project,
        PrivateChat
    }
}
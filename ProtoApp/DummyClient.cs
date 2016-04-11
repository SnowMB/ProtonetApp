using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using ProtoApp.Objects;
using GalaSoft.MvvmLight;

namespace ProtoApp
{
  public class DummyClient : ObservableObject, IProtonetClient
  {
    public bool IsAuthentificated => true;

    public string Token => "jkJh7(/6H/BgjkhlgGF/(43789KJgh";

    public Me User
    {
      get
      {
        throw new NotImplementedException ();
      }
    }

    public object Server
    {
      get
      {
        throw new NotImplementedException ();
      }
    }

    string IProtonetClient.Server
    {
      get
      {
        throw new NotImplementedException ();
      }
    }

        public string UsersUrl
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler AuthentificationComplete;
    public event EventHandler AuthentificationFailed;
    public event EventHandler LoggedOut;

    public async Task<bool> AuthentificateAsync ( string tokenString )
    {
      return await Task.FromResult ( true );
    }

    public async Task<bool> AuthentificateAsync ( string user, string password )
    {
      return await Task.FromResult ( true );
    }

    public void CancelAllRequests ()
    {
      return;
    }

    public Task<Meep> CreateFileMeepAsync ( string url, Stream file )
    {
      throw new NotImplementedException ();
    }

    public Task<Meep> CreateMeepAsync ( string url, MeepMessage meep )
    {
      throw new NotImplementedException ();
    }

    public async Task<PrivateChat> GetChatAsync ( string url )
    {
      return await Task.FromResult ( new PrivateChat ()
      {
        CreatedAt = DateTime.Now,
        CurrentMeepNumber = 0,
        ID = 0,
        LastMeep = await GetMeepAsync ( "" ),
        LastMeepDate = DateTime.Now.ToString (),
        MeepsUrl = "sdjlöal",
        NotificationID = 0,
        OtherUsers = new List<User> () { await GetUserAsync ( "" ) },
        OtherUser = await GetUserAsync ( "" ),
        UpdatedAt = DateTime.Now,
        Url = "abcdefghijklmnop"
      } );
    }

    public async Task<List<Meep>> GetMeepsAsync ( string url )
    {
      return new List<Meep> ()
            {
                await GetMeepAsync(""),
                await GetMeepAsync(""),
                await GetMeepAsync("")
            };
    }

    public async Task<List<PrivateChat>> GetChatsAsync ()
    {
      return new List<PrivateChat> ()
            {
                await GetChatAsync(""),
                await GetChatAsync(""),
                await GetChatAsync("")
            };
    }

    public async Task<Stream> GetDownloadStreamAsync ( string url )
    {
      return await Task.FromResult<Stream> ( null );
    }

    public async Task<Me> GetMeAsync ()
    {
      return await Task.FromResult ( new Me ()
      {
        Avatar = "",
        CreatedAt = DateTime.Now,
        Deactivated = false,
        DevicesUrl = "",
        Email = "em@i.l",
        FirstName = "Test",
        ID = 0,
        LastActiveAt = DateTime.Now,
        LastName = "User",
        Online = true,
        PrivateChatsUrl = "",
        ProjectsUrl = "",
        Role = "User",
        UpdatedAt = DateTime.Now,
        Url = "",
        UserName = "testuser",
        UserUrl = ""
      } );
    }

    public async Task<TokenResponse> GetTokenAsync ( string user, string password )
    {
      return await Task.FromResult ( new TokenResponse ()
      {
        Comment = "",
        CreatedAt = DateTime.Now,
        ID = 0,
        OwnerID = 0,
        UserID = 0,
        OwnerType = "User",
        Token = "jsdfsdfajklsdjkl",
        UpdatedAt = DateTime.Now,
        Url = ""
      } );
    }



    public void Logout ()
    {
      return;
    }

    public async Task<List<User>> GetUsersAsync ( string url )
    {
      return new List<User> ()
            {
                await GetUserAsync(""),
                await GetUserAsync(""),
                await GetUserAsync(""),
            };
    }

    public async Task<User> GetUserAsync ( string url )
    {
      return await Task.FromResult ( new User ()
      {
        Avatar = "",
        CreatedAt = DateTime.Now,
        Deactivated = false,
        Email = "em@i.l",
        FirstName = "Test",
        ID = 0,
        LastActiveAt = DateTime.Now,
        LastName = "User",
        Online = true,
        Role = "User",
        UpdatedAt = DateTime.Now,
        Url = "",
        UserName = "testuser"
      } );
    }

    public async Task<List<PrivateChat>> GetChatsAsync ( string url )
    {
      return new List<PrivateChat> ()
            {
                await GetChatAsync(""),
                await GetChatAsync(""),
                await GetChatAsync("")
            };
    }

    public async Task<Meep> GetMeepAsync ( string url )
    {
      return new Meep ()
      {
        CreatedAt = DateTime.Now,
        ID = 0,
        Files = null,
        Message = "blblbalblabal lbabdlauab af auidfh asii d",
        Number = 0,
        Type = "Meep",
        UpdatedAt = DateTime.Now,
        Url = "",
        User = await GetUserAsync ( "" )
      };
    }

    public Task<bool> AuthentificateAsync ( string server, string user, string password )
    {
      throw new NotImplementedException ();
    }

    public Task<Me> GetMeAsync ( string url )
    {
      throw new NotImplementedException ();
    }

    public Task CreatePushNotificationChannel ( string url )
    {
      throw new NotImplementedException ();
    }
  }
}

namespace ProtonetApp.Objects
{
    public class TokenResponse : ObjectBase
    {
        public string Token { get; set; }

        public int UserID { get; set; }

        public int OwnerID { get; set; }

        public string OwnerType { get; set; }

        public string Comment { get; set; }


    }
}
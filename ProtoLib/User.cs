namespace ProtoLib
{
    public class User : ObjectBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public string Role { get; set; }

        public bool Deactivated { get; set; }

        public string UserName { get; set; }

        public string LastActiveAt { get; set; }

        public bool Online { get; set; }
    }
}
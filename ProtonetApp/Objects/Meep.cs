namespace ProtonetApp.Objects
{
    public class Meep : ObjectBase
    {
        public string Type { get; set; }

        public int Number { get; set; }

        public string Message { get; set; }

        public User User { get; set; }
    }
}
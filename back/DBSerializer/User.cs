namespace DBSerializer
{
    //only for tests. internal class
    internal class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public override string ToString() => $"{Id} {Login} {Password} {Name} {Surname}";
    }
}

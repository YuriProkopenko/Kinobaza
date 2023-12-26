namespace Kinobaza.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string? Login { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        //waiting, banned, ok
        public string? Status { get; set; } = "waiting";
    }
}

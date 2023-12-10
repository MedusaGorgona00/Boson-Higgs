namespace BosonHiggsApi.DL.Entities
{
    public class Attempt
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public string? IpAddress { get; set; }
        

        public DateTime CreatedDateTime { get; set; }
    }
}

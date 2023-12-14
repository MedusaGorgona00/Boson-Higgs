namespace BosonHiggsApi.BL.Models.Options
{
    public class CorsOptions
    {
        public List<string> AllowedHosts { get; set; } = null!;

        public List<string> AllowedHeaders { get; set; } = null!;
    }
}

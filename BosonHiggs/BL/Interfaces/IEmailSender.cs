namespace BosonHiggsApi.BL.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body, string? from = null, bool isHtml = true);
    }
}
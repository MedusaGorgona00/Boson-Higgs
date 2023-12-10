using BosonHiggsApi.BL.Interfaces;
using BosonHiggsApi.BL.Models.EmailContents;

namespace BosonHiggsApi.BL.Services.Email
{
    public class AggregateEmailSender
    {
        private readonly IEmailSender _emailSender;

        public AggregateEmailSender(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task SendAsync<T>(T model, string toEmail) where T : EmailContent
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            if (toEmail is null) throw new ArgumentNullException(nameof(toEmail));

            await _emailSender.SendAsync(toEmail, model.GetSubject(), model.GetBody());
        }
    }
}
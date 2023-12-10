using BosonHiggsApi.BL.Enums;

namespace BosonHiggsApi.BL.Models.EmailContents
{
    public abstract class EmailContent
    {
        public EmailContent(EmailType emailType)
        {
            EmailType = emailType;
        }

        public EmailType EmailType { get; set; }
        public abstract string GetSubject();
        public abstract string GetBody();
    }
}

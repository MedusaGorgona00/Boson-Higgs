namespace BosonHiggsApi.BL.Models.Options
{
    public class EmailOptions
    {
        public string RequestsEmail { get; set; }
        public string RequestsEmailPassword { get; set; }

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public SMPTOptions SMTP { get; set; }


        public static void Validate(EmailOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("Option is empty.");

            if (string.IsNullOrEmpty(options.FromEmail))
                throw new ArgumentNullException("FromEmail is empty.");

            if (string.IsNullOrEmpty(options.FromName))
                throw new ArgumentNullException("FromName is empty.");

            if (options.SMTP != null)
            {
                if (string.IsNullOrEmpty(options.SMTP.Password))
                    throw new ArgumentNullException("SMTP.Password is empty.");

                if (string.IsNullOrEmpty(options.SMTP.Host))
                    throw new ArgumentNullException("SMTP.HOST is empty");

                if (!options.SMTP.Port.HasValue)
                    throw new ArgumentNullException("SMTP.Port is empty");
            }
            else
            {
                throw new Exception("If 'EnableEmailSender' is true, there should be the configuration of the email provider (SMTP or SendGrid).");
            }
        }

        public class SMPTOptions
        {
            public string Password { get; set; }
            public string Host { get; set; }
            public int? Port { get; set; }
        }
    }
}
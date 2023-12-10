using BosonHiggsApi.BL.Enums;

namespace BosonHiggsApi.BL.Models.EmailContents
{
    public class SendTokenEmailContent : EmailContent
    {
        public SendTokenEmailContent(string nick, string token) : base(EmailType.SendToken)
        {
            NickName = nick;
            Token = token;
        }

        public string NickName { get; set; }
        public string Token { get; set; }
        public override string GetSubject() => $"Got your token for game - Boson-Higgs";
        public override string GetBody() => $"<p>Hello {NickName}!</p> <b><i> Your token:</i></b> {Token} ";
    }
}
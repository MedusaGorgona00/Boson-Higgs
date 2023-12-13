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
        public override string GetBody() => $" <div style=\"background-color: #333333; width: 670px; height: 280px; border: 3px solid #00ff00;\">\r\n<pre><strong><span style=\"color: #00ff00;\">     __                                           __      _                           \r\n    / /_   ____    _____  ____    ____           / /_    (_)   ____ _   ____ _   _____\r\n   / __ \\ / __ \\  / ___/ / __ \\  / __ \\ ______  / __ \\  / /   / __ `/  / __ `/  / ___/\r\n  / /_/ // /_/ / (__  ) / /_/ / / / / //_____/ / / / / / /   / /_/ /  / /_/ /  (__  ) \r\n /_.___/ \\____/ /____/  \\____/ /_/ /_/        /_/ /_/ /_/    \\__, /   \\__, /  /____/  \r\n                                                           /____/   /____/            </span></strong></pre>\r\n<p style=\"color: #00ff00; font-family: Courier New;\"><strong>> user@boson-higgs: <span style=\"color: white;\">Hello, {NickName}!</span> </strong><br /><strong><span style=\"color: white;\">You've just registered for the game \"Boson-Higgs\".</span> </strong><br /><strong>> user@boson-higgs: <span style=\"color: white;\">Your user-token: {Token}. \r\n  <br><span style=\"color: #ff0000;\">Please, don't share it with anyone!</span></span> </strong><br /><br /><strong>> user@boson-higgs: <span style=\"color: white;\">Good luck!</span></strong></p>\r\n</div>";
    }
}
namespace BosonHiggsApi.BL.Helpers
{
    public static class AppConstants
    {
        public static readonly int ActivateHintInMinutes = 30;

        public static readonly int ActivateNextLevelHintInMinutes = 60;

        public static readonly string LastLevelLogin = "boson.higgs.devfest@gmail.com";

        public static readonly string LastLevelPassword = "DevFest3301MeduzaSimba"; //TODO: change

        public static readonly string AdminToken = Guid.NewGuid().ToString();

        public static readonly string BadWords = "prick, dick, cunt, pussy, pidor, suka, " +
                                                 "loh, gay, fuck, bitch, лох, жунбаш, гей, " +
                                                 "мырк, фак, пидор, пидарас, сука, блядь, " +
                                                 "еба, скейн, сгейн, далб, бля, пизд, хуй, blya, loh";
    }
}

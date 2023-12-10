namespace BosonHiggsApi.BL.Helpers
{
    public static class AppConstants
    {
        public static readonly int ActivateHintInMinutes = 30;

        public static readonly int ActivateNextLevelHintInMinutes = 60;

        public static readonly string LastLevelLogin = "boson.higgs.devfest@gmail.com";

        public static readonly string LastLevelPassword = "SimbaTest"; //TODO: change

        public static readonly string AdminToken = Guid.NewGuid().ToString();
    }
}

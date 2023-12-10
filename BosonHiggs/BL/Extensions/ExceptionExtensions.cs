using BosonHiggsApi.BL.Exceptions;

namespace BosonHiggsApi.BL.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Exception map to Dictionary
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> MapException(this Exception ex, string? field = null)
        {
            return new Dictionary<string, List<string>> { { field ?? string.Empty, new List<string> { ex.Message } } };
        }

        /// <summary>
        ///     Http exception map to Dictionary
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> MapException(this HttpException ex)
        {
            return new Dictionary<string, List<string>> { { ex.Property, ex.Errors } };
        }

        public static string JoinInnerExceptions(this Exception ex)
        {
            var message = ex.Message;
            if (ex.InnerException != null)
                message = $"{message}|###|{ex.InnerException.JoinInnerExceptions()}";

            return message;
        }
    }
}

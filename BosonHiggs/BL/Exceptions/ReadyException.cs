using System.Net;

namespace BosonHiggsApi.BL.Exceptions
{
    public class ReadyException : ApplicationException
    {
        /// <summary>
        /// </summary>
        private ReadyException(HttpStatusCode statusCode, Dictionary<string, List<string>> readyErrors)
        {
            StatusCode = statusCode;
            ReadyErrors = readyErrors;
        }

        /// <summary>
        ///     Ready errors
        /// </summary>
        public Dictionary<string, List<string>> ReadyErrors { get; set; }

        /// <summary>
        ///     Status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        ///     Creates exception
        /// </summary>
        /// <returns></returns>
        public static ReadyException Create(HttpStatusCode statusCode, Dictionary<string, List<string>> readyErrors)
        {
            return new ReadyException(statusCode, readyErrors);
        }

        /// <summary>
        ///     Throws exception
        /// </summary>
        /// <returns></returns>
        public static void Throw(HttpStatusCode statusCode, Dictionary<string, List<string>> readyErrors)
        {
            throw Create(statusCode, readyErrors);
        }
    }
}

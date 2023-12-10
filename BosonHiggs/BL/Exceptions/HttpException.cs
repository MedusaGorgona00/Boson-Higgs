using System.Net;

namespace BosonHiggsApi.BL.Exceptions
{
    /// <summary>
    ///     Http exception
    /// </summary>
    public class HttpException : ApplicationException
    {
        public const string DefaultErrorProperty = "Raw";

        /// <summary>
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="property"></param>
        private HttpException(HttpStatusCode statusCode, string message, string property = DefaultErrorProperty)
        {
            Property = property;
            StatusCode = statusCode;
            Errors = new List<string> { message };
        }

        /// <summary>
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="messages"></param>
        /// <param name="property"></param>
        private HttpException(HttpStatusCode statusCode, string[] messages, string property = DefaultErrorProperty)
        {
            Property = property;
            StatusCode = statusCode;
            Errors = messages.ToList();
        }

        /// <summary>
        ///     Property of messages
        /// </summary>
        public string Property { get; }

        /// <summary>
        ///     List of errors
        /// </summary>
        public List<string> Errors { get; }

        /// <summary>
        ///     Http status code
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        ///     creates exception
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static HttpException Create(HttpStatusCode statusCode, string message, string property = DefaultErrorProperty)
        {
            return new HttpException(statusCode, message, property);
        }

        /// <summary>
        ///     creates exception
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="messages"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static HttpException Create(HttpStatusCode statusCode, string[] messages, string property = DefaultErrorProperty)
        {
            return new HttpException(statusCode, messages, property);
        }
        
        /// <summary>
        ///     Throws exception
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="property"></param>
        public static void Throw(HttpStatusCode statusCode, string message, string property = DefaultErrorProperty)
        {
            throw Create(statusCode, message, property);
        }

        /// <summary>
        ///     Throws exception
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="messages"></param>
        /// <param name="property"></param>
        public static void Throw(HttpStatusCode statusCode, string[] messages, string property = DefaultErrorProperty)
        {
            throw Create(statusCode, messages, property);
        }

        /// <summary>
        ///     Throws exception if condition is true
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="property"></param>
        public static void ThrowIf(bool condition, HttpStatusCode statusCode, string message, string property = DefaultErrorProperty)
        {
            if (condition)
            {
                Throw(statusCode, message, property);
            }
        }

        /// <summary>
        ///     Throws exception if condition is true
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="statusCode"></param>
        /// <param name="messages"></param>
        /// <param name="property"></param>
        public static void ThrowIf(bool condition, HttpStatusCode statusCode, string[] messages, string property = DefaultErrorProperty)
        {
            if (condition)
            {
                Throw(statusCode, messages, property);
            }
        }

        /// <summary>
        ///     Throws exception if object is null
        /// </summary>
        /// <param name="object"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="property"></param>
        public static void ThrowIfNull(object @object, HttpStatusCode statusCode, string message, string property = DefaultErrorProperty)
        {
            ThrowIf(@object == null, statusCode, message, property);
        }
    }
}

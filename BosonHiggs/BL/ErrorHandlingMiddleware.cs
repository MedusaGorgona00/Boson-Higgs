using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Security.Authentication;
using BosonHiggsApi.BL.Exceptions;
using BosonHiggsApi.BL.Extensions;
using BosonHiggsApi.BL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BosonHiggsApi.BL
{
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        ///     Logging service interface
        /// </summary>
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        ///     A function that can process an HTTP request.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="next">A function that can process an HTTP request.</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        ///     Fulfill current requests
        /// </summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            var bodyAsText = string.Empty;
            try
            {
                if (context.Request.Headers.TryGetValue("Content-Type", out var headerValues)
                    && headerValues.Count > 0
                    && headerValues[0].Contains("application/json"))
                {
                    context.Request.EnableBuffering();
                    using var bodyReader = new StreamReader(context.Request.Body);

                    bodyAsText = await bodyReader.ReadToEndAsync();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);


                    await _next.Invoke(context);
                }
                else
                {
                    await _next.Invoke(context);
                }

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized || context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    _logger.LogInformation("Unauthorized or forbidden request.");
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, bodyAsText);
            }
        }

        /// <summary>
        ///     Handle the exception
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="bodyAsText"></param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception, string bodyAsText)
        {
            var test = context.Response.StatusCode; //TODO:  check
            var (code, message) = GetHttpStatusCodeAndMessage(exception);
            if (code >= HttpStatusCode.InternalServerError)
            {
                var fullRequestPath = $"{context.Request.Method} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                var str = $"Exception type: {exception.GetType()} \n Path: {fullRequestPath} \n Body: {bodyAsText} \n Message: {exception.Message} \n InnerException: {exception.JoinInnerExceptions()} \n"; // StackTrace: {exception.StackTrace}"; TODO:configure out do we need stacktrace 
                _logger.LogError(str);
            }
            else
            {
                _logger.LogError(exception, "UnhandledException");
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(GenerateResponseMessage(new ErrorModel(message)));
        }

        /// <summary>
        ///     Error conversion
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private (HttpStatusCode, Dictionary<string, List<string>>) GetHttpStatusCodeAndMessage(Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var errors = exception.MapException();
            switch (exception)
            {
                case BadRequestException exc:
                {
                    code = HttpStatusCode.BadRequest;
                    errors = exc.MapException();
                    break;
                }
                case NotFoundException exc:
                {
                    code = HttpStatusCode.NotFound;
                    errors = exc.MapException();
                    break;
                }
                case ForbiddenException exc:
                {
                    code = HttpStatusCode.Forbidden;
                    errors = exc.MapException();
                    break;
                }
                case ValidationException exc:
                {
                    code = HttpStatusCode.BadRequest;
                    errors = exc.MapException();
                    break;
                }
                case HttpException exc:
                {
                    code = exc.StatusCode;
                    errors = exc.MapException();
                    break;
                }
                case ReadyException exc:
                {
                    code = exc.StatusCode;
                    errors = exc.ReadyErrors;
                    break;
                }
                case ArgumentNullException _:
                case InvalidOperationException _:
                {
                    code = HttpStatusCode.BadRequest;
                    break;
                }
                case UnauthorizedAccessException _:
                case AuthenticationException _:
                {
                    code = HttpStatusCode.Unauthorized;
                    break;
                }
            }

            return (code, errors);
        }

        /// <summary>
        ///     Generating an error message
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string GenerateResponseMessage(object response)
        {
            return JsonConvert.SerializeObject(response,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}

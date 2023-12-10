using BosonHiggsApi.BL.Exceptions;
using FluentEmail.Core;
using Microsoft.Extensions.Options;
using System.Net;
using BosonHiggsApi.BL.Interfaces;
using BosonHiggsApi.BL.Models.Options;

namespace BosonHiggsApi.BL.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmailFactory _emailFactory;
        private readonly EmailOptions _options;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IFluentEmailFactory emailFactory,
            IOptions<EmailOptions> options, ILogger<EmailSender> logger)
        {
            _emailFactory = emailFactory ?? throw new ArgumentNullException(nameof(emailFactory));
            _logger = logger;
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task SendAsync(string to, string subject, string body, string? from, bool isHtml = false)
        {
            if (string.IsNullOrEmpty(to)) throw new ArgumentNullException(nameof(to));
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrEmpty(body)) throw new ArgumentNullException(nameof(body));

            var fromName = from ?? _options.FromEmail;

            var result = await _emailFactory
                .Create()
                .To(to)
                .SetFrom(_options.FromEmail, fromName)
                .Subject(subject)
                .Body(body, isHtml)
                .SendAsync();

            if (!result.Successful)
            {
                var error = $"EmailSender: {string.Join(", ", result.ErrorMessages)}";
                _logger.LogError(error);
                HttpException.ThrowIf(!result.Successful, HttpStatusCode.BadRequest, error);
            }
        }
    }
}

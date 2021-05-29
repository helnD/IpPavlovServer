using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Email
{
    /// <summary>
    /// Service for sending using Mailkit.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly SmtpConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        private const string QuestionSubject = "Вопрос с сайта";

        private const string QuestionTemplate = "Question.html";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">SMTP configuration.</param>
        /// <param name="logger">Logger object.</param>
        public EmailSender(IOptions<SmtpConfiguration> configuration, ILogger<EmailSender> logger)
        {
            _logger = logger;
            _configuration = configuration.Value;
        }

        /// <inheritdoc/>
        public async Task SendQuestionAsync(string name, string email, string question, CancellationToken cancellationToken)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "USERNAME", name },
                { "TEXT", question },
                { "EMAIL", email }
            };

            var mailInformation = new MailInformation
            {
                Name = name,
                Email = email,
                Subject = QuestionSubject,
                Placeholders = placeholders
            };

            await SendEmailByTemplate(QuestionTemplate, mailInformation, cancellationToken);
        }

        private async Task SendEmailByTemplate(string template, MailInformation mailInformation, CancellationToken cancellationToken)
        {
            var informationWithHtml = mailInformation with
            {
                HtmlBody = await ReadHtmlTemplate(template)
            };

            using var client = new SmtpClient();
            await ConnectAndAuthenticate(client, cancellationToken);

            var message = CreateMessage(informationWithHtml);
            await SendMessage(client, message, cancellationToken);

            await client.DisconnectAsync(true, cancellationToken);
        }

        private async Task<string> ReadHtmlTemplate(string templateName)
        {
            var path = Path.Combine(_configuration.PathToTemplates, templateName);
            using var fileStream = new StreamReader(path);
            return await fileStream.ReadToEndAsync();
        }

        private async Task ConnectAndAuthenticate(SmtpClient client, CancellationToken cancellationToken)
        {
            try
            {
                await client.ConnectAsync(_configuration.Domain, _configuration.Port, false, cancellationToken);
                await client.AuthenticateAsync(_configuration.Login, _configuration.Password, cancellationToken);
            }
            catch (SocketException exception)
            {
                _logger.LogWarning("Couldn't connect to server", exception);
                throw;
            }
            catch (AuthenticationException exception)
            {
                _logger.LogWarning("Couldn't authenticate on server", exception);
                throw;
            }
        }

        private async Task SendMessage(SmtpClient client, MimeMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await client.SendAsync(message, cancellationToken);
            }
            catch (CommandException exception)
            {
                _logger.LogError("Couldn't send email", exception);
            }
        }

        private MimeMessage CreateMessage(MailInformation mailInformation)
        {
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = ReplacePlaceholders(mailInformation.HtmlBody, mailInformation.Placeholders)
            };

            var message = new MimeMessage
            {
                Subject = mailInformation.Subject,
                Body = bodyBuilder.ToMessageBody()
            };

            message.From.Add(new MailboxAddress("ИП Павлов | Сайт", _configuration.Login));
            message.To.Add(new MailboxAddress(mailInformation.Name, _configuration.Login));

            return message;
        }

        private string ReplacePlaceholders(string html, Dictionary<string, string> arguments)
        {
            var result = new StringBuilder(html);
            foreach (var (name, value) in arguments)
            {
                result.Replace("{{" + name + "}}", value);
            }

            return result.ToString();
        }

        private record MailInformation
        {
            /// <summary>
            /// Recipient name.
            /// </summary>
            public string Name { get; init; }

            /// <summary>
            /// Recipient email.
            /// </summary>
            public string Email { get; init; }

            /// <summary>
            /// Email subject.
            /// </summary>
            public string Subject { get; init; }

            /// <summary>
            /// Placeholders for change template.
            /// </summary>
            public Dictionary<string, string> Placeholders { get; init; }

            /// <summary>
            /// Email HTML body.
            /// </summary>
            public string HtmlBody { get; init; }
        }
    }
}
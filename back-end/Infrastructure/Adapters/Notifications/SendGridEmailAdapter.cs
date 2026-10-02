using Application.Interfaces;
using Infrastructure.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Adapters.Notifications
{
    public class SendGridEmailAdapter : IEmailNotificationService
    {
        private readonly SendGridClientSdk _sendGridSdk;

        public SendGridEmailAdapter(SendGridClientSdk sendGridSdk)
        {
            _sendGridSdk = sendGridSdk;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body, CancellationToken ct = default)
        {
            // Translate domain request into SendGrid SDK payload format
            var payload = new SendGridPayload
            {
                ToEmail = recipientEmail,
                HeaderTitle = subject,
                ContentHtml = $"<p>{body}</p>"
            };

            var response = await _sendGridSdk.PostMessageAsync(payload);

            if (!response.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to send email via SendGrid. Status code: {response.StatusCode}");
            }
        }
    }
}

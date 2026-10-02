using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailNotificationService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body, CancellationToken ct = default);
    }
}

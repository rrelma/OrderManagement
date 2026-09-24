using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External
{
    public class SendGridClientSdk
    {
        public Task<SendGridResponseDto> PostMessageAsync(SendGridPayload payload)
        {
            Console.WriteLine($"[SENDGRID SDK] Direct HTTP Call -> To: {payload.ToEmail}, Subject: '{payload.HeaderTitle}'");
            return Task.FromResult(new SendGridResponseDto { StatusCode = 200, IsSuccess = true });
        }
    }

    public record SendGridPayload
    {
        public string ToEmail { get; init; } = string.Empty;
        public string HeaderTitle { get; init; } = string.Empty;
        public string ContentHtml { get; init; } = string.Empty;
    };

    public record SendGridResponseDto
    {
        public int StatusCode { get; init; }
        public bool IsSuccess { get; init; }
    }
}

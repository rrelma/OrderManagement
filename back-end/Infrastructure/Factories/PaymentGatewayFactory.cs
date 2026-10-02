using Application.Interfaces;
using Infrastructure.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Factories
{
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentGatewayFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentGateway GetPaymentGateway(string provider) => provider.ToLowerInvariant() switch
        {
            "stripe" => _serviceProvider.GetRequiredService<StripePaymentAdapter>(),
            "paypal" => _serviceProvider.GetRequiredService<PayPalPaymentAdapter>(),
            _ => throw new ArgumentException($"Unsupported payment provider: {provider}")
        };
    }
}

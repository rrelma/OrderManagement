using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pipelines
{
    public abstract class AbstractOrderValidator : IOrderValidator
    {
        private IOrderValidator? _nextValidator;

        public IOrderValidator SetNext(IOrderValidator next)
        {
            _nextValidator = next;
            return next;
        }

        public virtual async Task ValidateAsync(CreateOrderCommand command, CancellationToken ct = default)
        {
            if (_nextValidator != null)
            {
                await _nextValidator.ValidateAsync(command, ct);
            }
        }
    }
}

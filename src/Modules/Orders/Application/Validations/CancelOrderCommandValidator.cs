using FluentValidation;
using Modules.Orders.Application.Commands.CancelOrder;

namespace Modules.Orders.Application.Validation;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
        RuleFor(x => x.Reason).Must(r => r is null || r.Trim().Length <= 256);
    }
}
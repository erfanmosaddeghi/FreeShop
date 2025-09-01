using FluentValidation;
using Modules.Orders.Application.Commands.PayOrder;

namespace Modules.Orders.Application.Validation;

public sealed class PayOrderCommandValidator : AbstractValidator<PayOrderCommand>
{
    public PayOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
    }
}
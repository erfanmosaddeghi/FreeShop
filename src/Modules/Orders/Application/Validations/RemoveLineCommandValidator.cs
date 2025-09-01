using FluentValidation;
using Modules.Orders.Application.Commands.RemoveLine;

namespace Modules.Orders.Application.Validation;

public sealed class RemoveLineCommandValidator : AbstractValidator<RemoveLineCommand>
{
    public RemoveLineCommandValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
        RuleFor(x => x.LineNo).GreaterThan(0);
    }
}
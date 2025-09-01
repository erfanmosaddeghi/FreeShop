using FluentValidation;
using Modules.Orders.Application.Commands.UpdateLine;

namespace Modules.Orders.Application.Validation;

public sealed class UpdateLineCommandValidator : AbstractValidator<UpdateLineCommand>
{
    public UpdateLineCommandValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
        RuleFor(x => x.LineNo).GreaterThan(0);
        RuleFor(x => x.Quantity).Must(q => q is null || q > 0);
        RuleFor(x => x.UnitPriceRial).Must(p => p is null || p > 0);
        RuleFor(x => x).Must(x => x.Quantity is not null || x.UnitPriceRial is not null)
            .WithMessage("At least one of Quantity or UnitPriceRial must be provided.");
    }
}
using FluentValidation;
using Modules.Orders.Application.Commands.AddLine;

namespace Modules.Orders.Application.Validation;

public sealed class AddLineCommandValidator : AbstractValidator<AddLineCommand>
{
    public AddLineCommandValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPriceRial).GreaterThan(0);
    }
}
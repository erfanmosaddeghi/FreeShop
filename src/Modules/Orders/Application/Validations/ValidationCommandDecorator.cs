using FluentValidation;
using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Validation.Pipeline;

public sealed class ValidationCommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly IEnumerable<IValidator<TCommand>> _validators;
    private readonly ICommandHandler<TCommand, TResult> _inner;

    public ValidationCommandDecorator(IEnumerable<IValidator<TCommand>> validators, ICommandHandler<TCommand, TResult> inner)
    {
        _validators = validators;
        _inner = inner;
    }

    public async Task<TResult> Handle(TCommand command, CancellationToken ct)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TCommand>(command);
            var failures = new List<FluentValidation.Results.ValidationFailure>();
            foreach (var v in _validators)
            {
                var result = await v.ValidateAsync(context, ct);
                if (!result.IsValid) failures.AddRange(result.Errors);
            }
            if (failures.Count > 0) throw new ValidationException(failures);
        }

        return await _inner.Handle(command, ct);
    }
}
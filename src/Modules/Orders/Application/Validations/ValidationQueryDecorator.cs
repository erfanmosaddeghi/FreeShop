using FluentValidation;
using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Validation.Pipeline;

public sealed class ValidationQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly IEnumerable<IValidator<TQuery>> _validators;
    private readonly IQueryHandler<TQuery, TResult> _inner;

    public ValidationQueryDecorator(IEnumerable<IValidator<TQuery>> validators, IQueryHandler<TQuery, TResult> inner)
    {
        _validators = validators;
        _inner = inner;
    }

    public async Task<TResult> Handle(TQuery query, CancellationToken ct)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TQuery>(query);
            var failures = new List<FluentValidation.Results.ValidationFailure>();
            foreach (var v in _validators)
            {
                var result = await v.ValidateAsync(context, ct);
                if (!result.IsValid) failures.AddRange(result.Errors);
            }
            if (failures.Count > 0) throw new ValidationException(failures);
        }

        return await _inner.Handle(query, ct);
    }
}
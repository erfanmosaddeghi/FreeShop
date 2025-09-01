using MediatR;

namespace Modules.Orders.Application.Abstractions.CQRS2;

public interface ICommand<TResult> : IRequest<TResult> { }
public interface IQuery<TResult> : IRequest<TResult> { }
public interface ITransactionalCommand<TResult> : ICommand<TResult> { }
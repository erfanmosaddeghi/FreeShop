using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.Abstractions.CQRS2;
using Modules.Orders.Application.Ports;
using Modules.Orders.Infrastructure.Persistence;
using SharedKernel.Domain;

namespace Modules.Orders.Infrastructure.Pipeline;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalCommand<TResponse>
{
    private readonly OrdersDbContext _dbContext;
    private readonly IUnitOfWork _uow;

    public TransactionBehavior(OrdersDbContext dbContext, IUnitOfWork uow)
    {
        _dbContext = dbContext;
        _uow = uow;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_dbContext.Database.CurrentTransaction is not null)
        {
            return await next();
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next();

            await _uow.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
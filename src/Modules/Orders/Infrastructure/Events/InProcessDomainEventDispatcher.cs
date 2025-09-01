using System.Reflection;
using SharedKernel.Domain;

namespace Modules.Orders.Infrastructure.Events;

public sealed class InProcessDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _sp;

    public InProcessDomainEventDispatcher(IServiceProvider sp)
    {
        _sp = sp;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        foreach (var ev in events)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(ev.GetType());
            var handlers = (IEnumerable<object>) (_sp.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType)) ?? Array.Empty<object>());
            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod("HandleAsync", BindingFlags.Public | BindingFlags.Instance)!;
                var task = (Task)method.Invoke(handler, new object?[] { ev, ct })!;
                await task.ConfigureAwait(false);
            }
        }
    }
}
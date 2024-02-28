using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RedisChannels;

public class EventHandlerWrapper
{
    private readonly Func<RedisValue, IServiceProvider, CancellationToken, Task> handleTaskFunc;

    private EventHandlerWrapper(Type eventHandlerType, Func<RedisValue, IServiceProvider, CancellationToken, Task> handleTaskFunc)
    {
        this.handleTaskFunc = handleTaskFunc;

        EventHandlerType = eventHandlerType;
    }

    public Type EventHandlerType { get; }

    public void OnNewMessage(RedisValue redisValue, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        Task.Factory.StartNew(
            async _ => await handleTaskFunc.Invoke(redisValue, serviceProvider, cancellationToken).ConfigureAwait(false),
            TaskCreationOptions.LongRunning,
            cancellationToken
        );
    }

    internal static EventHandlerWrapper Create<TEvent, TEventHandler>()
        where TEvent : class
        where TEventHandler : IHandleRedisChannelsEvents<TEvent>
    {
        var typedHandleTaskFunc = BuildTypedHandleTaskFunc<TEvent, TEventHandler>();

        return new EventHandlerWrapper(typeof(TEventHandler), typedHandleTaskFunc);
    }

    private static Func<RedisValue, IServiceProvider, CancellationToken, Task> BuildTypedHandleTaskFunc<TEvent, TEventHandler>()
        where TEvent : class 
        where TEventHandler : IHandleRedisChannelsEvents<TEvent>
    {
        return async (redisValue, serviceProvider, cancellationToken) =>
        {
            await using var scope = serviceProvider.CreateAsyncScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<EventHandlerWrapper>>();

            try
            {
                var @event = JsonSerializer.Deserialize<TEvent>(redisValue.ToString()) ?? throw new Exception("Could not deserialize");

                var messageName = @event.GetType().Name;

                logger.LogInformation("Handling {message}", messageName);

                var handler = scope.ServiceProvider.GetRequiredService<TEventHandler>();

                await handler.HandleAsync(@event, cancellationToken).ConfigureAwait(false);

                logger.LogInformation("Handled {message}", messageName);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception handling message: {ex} {innerEx}", ex.Message, ex.InnerException?.Message);

                // todo: handle failed message
            }
        };
    }
}
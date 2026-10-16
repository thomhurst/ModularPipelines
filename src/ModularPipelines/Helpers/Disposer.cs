using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Helpers;

/// <summary>
/// Provides utility methods for disposing objects that may implement <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>.
/// </summary>
internal static class Disposer
{
    /// <summary>
    /// Asynchronously disposes an object if it implements <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="obj">The object to dispose. Can be null.</param>
    /// <returns>A task representing the asynchronous dispose operation.</returns>
    public static async Task DisposeObjectAsync(object? obj)
    {
        if (obj is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else if (obj is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    /// <summary>
    /// Registers an object to be disposed when the process exits.
    /// </summary>
    /// <remarks>
    /// The process-exit handler waits up to 30 seconds for disposal to complete.
    /// Exceptions during disposal are suppressed since the process is exiting.
    /// </remarks>
    /// <param name="obj">The object to dispose on process exit. Can be null.</param>
    [ExcludeFromCodeCoverage]
    public static void RegisterOnShutdown(object? obj)
    {
        _ = RegisterOnShutdownWithUnregistration(obj);
    }

    internal static IDisposable RegisterOnShutdownWithUnregistration(object? obj) =>
        RegisterOnShutdownWithUnregistration(() => DisposeObjectAsync(obj));

    internal static IDisposable RegisterOnShutdownWithUnregistration(Func<Task> shutdown) =>
        RegisterOnShutdownWithUnregistration(
            shutdown,
            handler => AppDomain.CurrentDomain.ProcessExit += handler,
            handler => AppDomain.CurrentDomain.ProcessExit -= handler);

    internal static IDisposable RegisterOnShutdownWithUnregistration(
        object? obj,
        Action<EventHandler> subscribe,
        Action<EventHandler> unsubscribe) =>
        RegisterOnShutdownWithUnregistration(() => DisposeObjectAsync(obj), subscribe, unsubscribe);

    internal static IDisposable RegisterOnShutdownWithUnregistration(
        Func<Task> shutdown,
        Action<EventHandler> subscribe,
        Action<EventHandler> unsubscribe,
        TimeSpan? timeout = null)
    {
        ArgumentNullException.ThrowIfNull(shutdown);
        ArgumentNullException.ThrowIfNull(subscribe);
        ArgumentNullException.ThrowIfNull(unsubscribe);
        return new ShutdownRegistration(shutdown, subscribe, unsubscribe, timeout ?? TimeSpan.FromSeconds(30));
    }

    /// <summary>
    /// Runs shutdown outside any caller synchronization context and bounds the entire operation,
    /// including synchronous cancellation callbacks and synchronous disposal.
    /// </summary>
    /// <param name="shutdown">The shutdown operation.</param>
    /// <param name="timeout">The maximum time to wait.</param>
    [ExcludeFromCodeCoverage]
    private static void DisposeOnShutdown(Func<Task> shutdown, TimeSpan timeout)
    {
        try
        {
            Task.Run(shutdown).WaitAsync(timeout).GetAwaiter().GetResult();
        }
        catch (ObjectDisposedException)
        {
            // Expected - object may already be disposed during shutdown
        }
        catch (OperationCanceledException)
        {
            // Expected - operations may be canceled during shutdown
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            // Suppress all other exceptions during shutdown - process is exiting anyway
            // and there's no meaningful way to handle or report them at this point
        }
    }

    private sealed class ShutdownRegistration : IDisposable
    {
        private readonly Action<EventHandler> _unsubscribe;
        private EventHandler? _handler;

        public ShutdownRegistration(
            Func<Task> shutdown,
            Action<EventHandler> subscribe,
            Action<EventHandler> unsubscribe,
            TimeSpan timeout)
        {
            _unsubscribe = unsubscribe;
            _handler = (_, _) => DisposeOnShutdown(shutdown, timeout);
            subscribe(_handler);
        }

        public void Dispose()
        {
            var handler = Interlocked.Exchange(ref _handler, null);
            if (handler is not null)
            {
                _unsubscribe(handler);
            }
        }
    }
}

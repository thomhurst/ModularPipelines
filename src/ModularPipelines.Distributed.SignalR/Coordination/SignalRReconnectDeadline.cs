using Microsoft.Extensions.Logging;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Bounds the whole recovery, including a stalled connection attempt or registration invocation.
/// </summary>
internal sealed class SignalRReconnectDeadline(
    TimeSpan timeout,
    Func<Task> onExpired,
    ILogger logger) : IDisposable
{
    private readonly Lock _lock = new();
    private CancellationTokenSource? _deadline;
    private bool _disposed;

    internal CancellationToken Start()
    {
        lock (_lock)
        {
            if (_disposed)
            {
                // A connection callback can already be queued when disposal removes its handler.
                return new CancellationToken(canceled: true);
            }

            if (_deadline is not null)
            {
                return _deadline.Token;
            }

            var deadline = new CancellationTokenSource();
            _deadline = deadline;
            var token = deadline.Token;
            _ = ExpireAsync(deadline, token, Task.Delay(timeout, token));
            return token;
        }
    }

    /// <summary>
    /// Completes recovery only if the deadline has not already expired.
    /// </summary>
    internal bool TryComplete()
    {
        lock (_lock)
        {
            if (_disposed || _deadline?.IsCancellationRequested == true)
            {
                return false;
            }

            var deadline = _deadline;
            _deadline = null;
            deadline?.Cancel();
            deadline?.Dispose();
            return true;
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _disposed = true;
            var deadline = _deadline;
            _deadline = null;
            deadline?.Cancel();
            deadline?.Dispose();
        }
    }

    private async Task ExpireAsync(CancellationTokenSource deadline, CancellationToken token, Task delay)
    {
        // Keep expiry callbacks outside Start's caller, even for a zero-duration deadline.
        await Task.CompletedTask.ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
        try
        {
            await delay.ConfigureAwait(false);
            lock (_lock)
            {
                if (!ReferenceEquals(_deadline, deadline))
                {
                    return;
                }

                deadline.Cancel();
            }

            await onExpired().ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            // Successful recovery or disposal cancels the pending deadline.
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not stop the worker connection after its reconnect deadline expired");
        }
        finally
        {
            lock (_lock)
            {
                // Keep an expired source usable by repeated Start calls until disposal.
                if (!ReferenceEquals(_deadline, deadline))
                {
                    deadline.Dispose();
                }
            }
        }
    }
}

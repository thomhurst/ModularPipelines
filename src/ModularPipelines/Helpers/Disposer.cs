using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Helpers;

/// <summary>
/// Provides utility methods for disposing objects that may implement <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>.
/// </summary>
public static class Disposer
{
    /// <summary>
    /// The default timeout for synchronous disposal operations.
    /// Set to 1 second to accommodate the limited time available during process exit (typically 2 seconds).
    /// </summary>
    public static readonly TimeSpan DefaultDisposeTimeout = TimeSpan.FromSeconds(1);

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
    /// Synchronously disposes an object with the default timeout.
    /// </summary>
    /// <remarks>
    /// This method uses a timeout to prevent indefinite blocking during process shutdown.
    /// The default timeout is 1 second, which allows disposal to complete while leaving
    /// buffer time within the typical 2-second process exit window.
    /// If the disposal does not complete within the timeout, the method returns without
    /// waiting further. The disposal operation may continue in the background but is not
    /// guaranteed to complete.
    /// </remarks>
    /// <param name="obj">The object to dispose. Can be null.</param>
    public static void DisposeObject(object? obj) => DisposeObject(obj, DefaultDisposeTimeout);

    /// <summary>
    /// Synchronously disposes an object with a custom timeout.
    /// </summary>
    /// <remarks>
    /// This method uses a timeout to prevent indefinite blocking during process shutdown.
    /// If the disposal does not complete within the specified timeout, the method returns
    /// without waiting further. The disposal operation may continue in the background but
    /// is not guaranteed to complete.
    /// </remarks>
    /// <param name="obj">The object to dispose. Can be null.</param>
    /// <param name="timeout">The maximum time to wait for disposal to complete.</param>
    public static void DisposeObject(object? obj, TimeSpan timeout)
    {
        if (obj is null)
        {
            return;
        }

        var disposeTask = DisposeObjectAsync(obj);

        // Use Task.Wait with timeout to prevent indefinite blocking
        // This is particularly important during ProcessExit which has limited time
        try
        {
            disposeTask.Wait(timeout);
        }
        catch (AggregateException ex)
        {
            // Unwrap and rethrow the inner exception to preserve the original exception type
            if (ex.InnerExceptions.Count == 1)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
            }

            throw;
        }
    }

    /// <summary>
    /// Registers an object to be disposed when the process exits.
    /// </summary>
    /// <remarks>
    /// The disposal will use <see cref="DefaultDisposeTimeout"/> to prevent blocking
    /// during the limited process exit window.
    /// </remarks>
    /// <param name="obj">The object to dispose on process exit. Can be null.</param>
    [ExcludeFromCodeCoverage]
    public static void RegisterOnShutdown(object? obj)
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) => DisposeObject(obj);
    }
}
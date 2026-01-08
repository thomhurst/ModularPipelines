using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Helpers;

/// <summary>
/// Provides utility methods for disposing objects that may implement <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>.
/// </summary>
public static class Disposer
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
    /// The disposal is performed asynchronously using a fire-and-forget pattern.
    /// Since ProcessExit handlers have limited time (typically 2 seconds),
    /// the disposal is started but may not complete if it takes too long.
    /// Exceptions during disposal are suppressed since the process is exiting.
    /// </remarks>
    /// <param name="obj">The object to dispose on process exit. Can be null.</param>
    [ExcludeFromCodeCoverage]
    public static void RegisterOnShutdown(object? obj)
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) => _ = DisposeOnShutdownAsync(obj);
    }

    /// <summary>
    /// Internal async disposal for shutdown scenarios.
    /// </summary>
    /// <param name="obj">The object to dispose.</param>
    /// <returns>A task representing the disposal operation.</returns>
    [ExcludeFromCodeCoverage]
    private static async Task DisposeOnShutdownAsync(object? obj)
    {
        try
        {
            await DisposeObjectAsync(obj).ConfigureAwait(false);
        }
        catch (ObjectDisposedException)
        {
            // Expected - object may already be disposed during shutdown
        }
        catch (OperationCanceledException)
        {
            // Expected - operations may be cancelled during shutdown
        }
        catch (Exception)
        {
            // Suppress all other exceptions during shutdown - process is exiting anyway
            // and there's no meaningful way to handle or report them at this point
        }
    }
}
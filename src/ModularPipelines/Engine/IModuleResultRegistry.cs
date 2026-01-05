using System.Collections.Concurrent;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Registry for storing and retrieving module execution results.
/// Used for coordinating results between pure IModule&lt;T&gt; implementations.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> Implementations of this interface must be thread-safe.
/// All methods can be called concurrently from multiple threads without external synchronization.
/// </para>
/// </remarks>
internal interface IModuleResultRegistry
{
    /// <summary>
    /// Registers a module result.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="moduleType">The module type.</param>
    /// <param name="result">The module result.</param>
    void RegisterResult<T>(Type moduleType, ModuleResult<T> result);

    /// <summary>
    /// Gets a module result if available.
    /// </summary>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <param name="moduleType">The module type.</param>
    /// <returns>The module result, or null if not yet available.</returns>
    ModuleResult<T>? GetResult<T>(Type moduleType);

    /// <summary>
    /// Gets a module result if available (non-generic version).
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <returns>The module result as IModuleResult, or null if not yet available.</returns>
    IModuleResult? GetResult(Type moduleType);

    /// <summary>
    /// Gets the task that completes when a module finishes.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <returns>The completion task, or null if the module is not registered.</returns>
    Task? GetCompletionTask(Type moduleType);

    /// <summary>
    /// Registers a module for result tracking.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    void RegisterModule(Type moduleType);

    /// <summary>
    /// Sets a module as completed with an exception.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    void SetException(Type moduleType, Exception exception);

    /// <summary>
    /// Registers a module result (non-generic version).
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="result">The module result.</param>
    void RegisterResult(Type moduleType, IModuleResult result);
}

/// <summary>
/// Default implementation of the module result registry.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All public methods can be called
/// concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// Uses <see cref="ConcurrentDictionary{TKey,TValue}"/> for lock-free reads and thread-safe writes.
/// The <see cref="TaskCompletionSource{TResult}"/> provides memory barriers ensuring visibility
/// of result writes to awaiting threads.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class ModuleResultRegistry : IModuleResultRegistry
{
    private readonly ConcurrentDictionary<Type, object> _results = new();
    private readonly ConcurrentDictionary<Type, TaskCompletionSource<object?>> _completionSources = new();

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Uses <see cref="ConcurrentDictionary{TKey,TValue}.GetOrAdd"/>
    /// for atomic registration.
    /// </remarks>
    public void RegisterModule(Type moduleType)
    {
        _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. The result is stored before signaling completion,
    /// and TrySetResult provides release semantics ensuring visibility to awaiters.
    /// </remarks>
    public void RegisterResult<T>(Type moduleType, ModuleResult<T> result)
    {
        // Store result first, then signal completion
        // TrySetResult provides release semantics, ensuring _results write is visible to awaiters
        _results[moduleType] = result;
        var tcs = _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());
        tcs.TrySetResult(result);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Uses lock-free reads from ConcurrentDictionary.
    /// </remarks>
    public ModuleResult<T>? GetResult<T>(Type moduleType)
    {
        if (_results.TryGetValue(moduleType, out var result) && result is ModuleResult<T> typedResult)
        {
            return typedResult;
        }

        return null;
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Uses lock-free reads from ConcurrentDictionary.
    /// </remarks>
    public IModuleResult? GetResult(Type moduleType)
    {
        if (_results.TryGetValue(moduleType, out var result))
        {
            return result as IModuleResult;
        }

        return null;
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Uses lock-free reads from ConcurrentDictionary.
    /// </remarks>
    public Task? GetCompletionTask(Type moduleType)
    {
        return _completionSources.TryGetValue(moduleType, out var tcs) ? tcs.Task : null;
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Uses TrySetException for atomic exception signaling.
    /// </remarks>
    public void SetException(Type moduleType, Exception exception)
    {
        // Only set exception if module was previously registered (preserves original behavior)
        if (_completionSources.TryGetValue(moduleType, out var tcs))
        {
            tcs.TrySetException(exception);
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. The result is stored before signaling completion,
    /// and TrySetResult provides release semantics ensuring visibility to awaiters.
    /// </remarks>
    public void RegisterResult(Type moduleType, IModuleResult result)
    {
        // Store result first, then signal completion
        // TrySetResult provides release semantics, ensuring _results write is visible to awaiters
        _results[moduleType] = result;
        var tcs = _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());
        tcs.TrySetResult(result);
    }
}

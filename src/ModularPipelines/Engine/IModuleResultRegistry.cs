using System.Collections.Concurrent;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Registry for storing and retrieving module execution results.
/// Used for coordinating results between pure IModule&lt;T&gt; implementations.
/// </summary>
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
    /// Creates a TaskCompletionSource on-demand if the module is not yet registered.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <returns>The completion task.</returns>
    Task GetCompletionTask(Type moduleType);

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
/// Uses ConcurrentDictionary for lock-free reads and thread-safe writes.
/// </summary>
internal class ModuleResultRegistry : IModuleResultRegistry
{
    private readonly ConcurrentDictionary<Type, object> _results = new();
    private readonly ConcurrentDictionary<Type, TaskCompletionSource<object?>> _completionSources = new();

    public void RegisterModule(Type moduleType)
    {
        _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());
    }

    public void RegisterResult<T>(Type moduleType, ModuleResult<T> result)
    {
        // Store result first, then signal completion
        // TrySetResult provides release semantics, ensuring _results write is visible to awaiters
        _results[moduleType] = result;
        var tcs = _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());
        tcs.TrySetResult(result);
    }

    public ModuleResult<T>? GetResult<T>(Type moduleType)
    {
        if (_results.TryGetValue(moduleType, out var result) && result is ModuleResult<T> typedResult)
        {
            return typedResult;
        }

        return null;
    }

    public IModuleResult? GetResult(Type moduleType)
    {
        if (_results.TryGetValue(moduleType, out var result))
        {
            return result as IModuleResult;
        }

        return null;
    }

    public Task GetCompletionTask(Type moduleType)
    {
        // Use GetOrAdd for consistency with SetException - creates TCS on-demand
        return _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>()).Task;
    }

    public void SetException(Type moduleType, Exception exception)
    {
        var tcs = _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());
        tcs.TrySetException(exception);
    }

    public void RegisterResult(Type moduleType, IModuleResult result)
    {
        // Store result first, then signal completion
        // TrySetResult provides release semantics, ensuring _results write is visible to awaiters
        _results[moduleType] = result;
        var tcs = _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());
        tcs.TrySetResult(result);
    }
}

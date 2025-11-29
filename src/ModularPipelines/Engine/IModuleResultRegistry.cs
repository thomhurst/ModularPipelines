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
}

/// <summary>
/// Default implementation of the module result registry.
/// </summary>
internal class ModuleResultRegistry : IModuleResultRegistry
{
    private readonly Dictionary<Type, object> _results = new();
    private readonly Dictionary<Type, TaskCompletionSource<object?>> _completionSources = new();
    private readonly object _lock = new();

    public void RegisterModule(Type moduleType)
    {
        lock (_lock)
        {
            if (!_completionSources.ContainsKey(moduleType))
            {
                _completionSources[moduleType] = new TaskCompletionSource<object?>();
            }
        }
    }

    public void RegisterResult<T>(Type moduleType, ModuleResult<T> result)
    {
        lock (_lock)
        {
            _results[moduleType] = result;

            if (_completionSources.TryGetValue(moduleType, out var tcs))
            {
                tcs.TrySetResult(result);
            }
        }
    }

    public ModuleResult<T>? GetResult<T>(Type moduleType)
    {
        lock (_lock)
        {
            if (_results.TryGetValue(moduleType, out var result) && result is ModuleResult<T> typedResult)
            {
                return typedResult;
            }

            return null;
        }
    }

    public Task? GetCompletionTask(Type moduleType)
    {
        lock (_lock)
        {
            return _completionSources.TryGetValue(moduleType, out var tcs) ? tcs.Task : null;
        }
    }

    public void SetException(Type moduleType, Exception exception)
    {
        lock (_lock)
        {
            if (_completionSources.TryGetValue(moduleType, out var tcs))
            {
                tcs.TrySetException(exception);
            }
        }
    }
}

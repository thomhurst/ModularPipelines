using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Indicates the result of a module result lookup operation.
/// </summary>
internal enum ModuleResultLookupStatus
{
    /// <summary>
    /// The result was found and is of the expected type.
    /// </summary>
    Found,

    /// <summary>
    /// No result has been registered for the module yet.
    /// </summary>
    NotFound,

    /// <summary>
    /// A result exists but is not of the expected type.
    /// </summary>
    TypeMismatch,
}

/// <summary>
/// Represents the result of a module result lookup operation,
/// distinguishing between "not found", "wrong type", and "found with value".
/// </summary>
/// <typeparam name="T">The expected result type.</typeparam>
internal readonly struct ModuleResultLookup<T>
{
    /// <summary>
    /// Gets the status of the lookup operation.
    /// </summary>
    public ModuleResultLookupStatus Status { get; }

    /// <summary>
    /// Gets the result value if <see cref="Status"/> is <see cref="ModuleResultLookupStatus.Found"/>.
    /// </summary>
    public ModuleResult<T>? Value { get; }

    /// <summary>
    /// Gets a value indicating whether the lookup was successful.
    /// </summary>
    public bool IsFound => Status == ModuleResultLookupStatus.Found;

    private ModuleResultLookup(ModuleResultLookupStatus status, ModuleResult<T>? value)
    {
        Status = status;
        Value = value;
    }

    /// <summary>
    /// Creates a successful lookup result.
    /// </summary>
    public static ModuleResultLookup<T> Found(ModuleResult<T> value) =>
        new(ModuleResultLookupStatus.Found, value);

    /// <summary>
    /// Creates a not-found lookup result.
    /// </summary>
    public static ModuleResultLookup<T> NotFound() =>
        new(ModuleResultLookupStatus.NotFound, default);

    /// <summary>
    /// Creates a type-mismatch lookup result.
    /// </summary>
    public static ModuleResultLookup<T> TypeMismatch() =>
        new(ModuleResultLookupStatus.TypeMismatch, default);
}

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
    /// Attempts to get a module result with detailed status information.
    /// </summary>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <param name="moduleType">The module type.</param>
    /// <returns>A lookup result that distinguishes between "not found", "type mismatch", and "found".</returns>
    ModuleResultLookup<T> TryGetResult<T>(Type moduleType);

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
    /// <para>
    /// This method is thread-safe. Uses explicit memory barriers to ensure result visibility
    /// before signaling completion.
    /// </para>
    /// <para>
    /// The sequence is:
    /// 1. Store result in dictionary
    /// 2. Memory barrier ensures the write is visible to other threads
    /// 3. Signal completion via TCS
    /// </para>
    /// </remarks>
    public void RegisterResult<T>(Type moduleType, ModuleResult<T> result)
    {
        // Get or create TCS first - this ensures it exists before we store the result
        // Critical: TCS must be retrieved BEFORE storing the result to ensure proper
        // happens-before ordering when awaiters check _results after TCS completion
        var tcs = _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());

        // Store result
        _results[moduleType] = result;

        // Explicit memory barrier to ensure the result write is visible
        // to all threads before we signal completion
        Thread.MemoryBarrier();

        // Now signal completion - awaiters are guaranteed to see the result
        tcs.TrySetResult(result);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Uses lock-free reads from ConcurrentDictionary.
    /// Returns null if not found or type mismatch. Use <see cref="TryGetResult{T}"/>
    /// for detailed status information.
    /// </remarks>
    public ModuleResult<T>? GetResult<T>(Type moduleType)
    {
        var lookup = TryGetResult<T>(moduleType);
        return lookup.IsFound ? lookup.Value : null;
    }

    /// <inheritdoc />
    /// <remarks>
    /// <para>
    /// This method is thread-safe. Uses lock-free reads from ConcurrentDictionary.
    /// </para>
    /// <para>
    /// Returns a <see cref="ModuleResultLookup{T}"/> that distinguishes between:
    /// <list type="bullet">
    /// <item><see cref="ModuleResultLookupStatus.Found"/> - Result exists and matches type</item>
    /// <item><see cref="ModuleResultLookupStatus.NotFound"/> - No result registered for module</item>
    /// <item><see cref="ModuleResultLookupStatus.TypeMismatch"/> - Result exists but wrong type</item>
    /// </list>
    /// </para>
    /// </remarks>
    public ModuleResultLookup<T> TryGetResult<T>(Type moduleType)
    {
        if (!_results.TryGetValue(moduleType, out var result))
        {
            return ModuleResultLookup<T>.NotFound();
        }

        if (result is ModuleResult<T> typedResult)
        {
            return ModuleResultLookup<T>.Found(typedResult);
        }

        return ModuleResultLookup<T>.TypeMismatch();
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
    /// <para>
    /// This method is thread-safe. Uses explicit memory barriers to ensure result visibility
    /// before signaling completion.
    /// </para>
    /// <para>
    /// The sequence is:
    /// 1. Store result in dictionary
    /// 2. Memory barrier ensures the write is visible to other threads
    /// 3. Signal completion via TCS
    /// </para>
    /// </remarks>
    public void RegisterResult(Type moduleType, IModuleResult result)
    {
        // Get or create TCS first - this ensures it exists before we store the result
        // Critical: TCS must be retrieved BEFORE storing the result to ensure proper
        // happens-before ordering when awaiters check _results after TCS completion
        var tcs = _completionSources.GetOrAdd(moduleType, _ => new TaskCompletionSource<object?>());

        // Store result
        _results[moduleType] = result;

        // Explicit memory barrier to ensure the result write is visible
        // to all threads before we signal completion
        Thread.MemoryBarrier();

        // Now signal completion - awaiters are guaranteed to see the result
        tcs.TrySetResult(result);
    }
}

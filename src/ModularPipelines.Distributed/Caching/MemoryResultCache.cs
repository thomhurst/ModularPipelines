using System.Collections.Concurrent;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Caching;

/// <summary>
/// In-memory implementation of <see cref="IResultCache"/> for caching module results.
/// Thread-safe for concurrent access.
/// </summary>
internal sealed class MemoryResultCache : IResultCache
{
    private readonly ConcurrentDictionary<string, IModuleResult> _cache = new();

    /// <inheritdoc />
    public Task SetResultAsync(
        Type moduleType,
        IModuleResult result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(moduleType);
        ArgumentNullException.ThrowIfNull(result);

        var key = GetKey(moduleType);
        _cache[key] = result;

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IModuleResult?> GetResultAsync(
        Type moduleType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(moduleType);

        var key = GetKey(moduleType);
        _cache.TryGetValue(key, out var result);

        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<bool> ContainsResultAsync(
        Type moduleType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(moduleType);

        var key = GetKey(moduleType);
        return Task.FromResult(_cache.ContainsKey(key));
    }

    /// <inheritdoc />
    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        _cache.Clear();
        return Task.CompletedTask;
    }

    private static string GetKey(Type moduleType)
    {
        return moduleType.AssemblyQualifiedName ?? moduleType.FullName ?? moduleType.Name;
    }
}

using System.Runtime.CompilerServices;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Master;

internal sealed class DistributedCacheHitTracker
{
    private readonly ConditionalWeakTable<IModuleResult, CacheHitMarker> _results = new();

    public void Record(IModuleResult result) =>
        _results.GetValue(result, static _ => CacheHitMarker.Instance);

    public bool Contains(IModuleResult? result) =>
        result is not null && _results.TryGetValue(result, out _);

    private sealed class CacheHitMarker
    {
        public static CacheHitMarker Instance { get; } = new();
    }
}

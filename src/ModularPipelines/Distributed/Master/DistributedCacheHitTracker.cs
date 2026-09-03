using System.Collections.Concurrent;

namespace ModularPipelines.Distributed.Master;

internal sealed class DistributedCacheHitTracker
{
    private readonly ConcurrentDictionary<Type, byte> _moduleTypes = new();

    public void Record(Type moduleType) => _moduleTypes.TryAdd(moduleType, 0);

    public bool Contains(Type moduleType) => _moduleTypes.ContainsKey(moduleType);

    public void Clear() => _moduleTypes.Clear();
}

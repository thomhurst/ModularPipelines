using System.Collections.Concurrent;

namespace ModularPipelines.Distributed;

internal sealed class DependencyResultCache(
    IDistributedWorkerCoordinator coordinator,
    CancellationToken cancellationToken)
{
    private readonly ConcurrentDictionary<ModuleId, Lazy<Task<SerializedModuleResult>>> _results = new();

    public async Task<SerializedModuleResult> GetAsync(ModuleId moduleId)
    {
        var lazyResult = _results.GetOrAdd(
            moduleId,
            name => new Lazy<Task<SerializedModuleResult>>(
                () => coordinator.WaitForResultAsync(name, cancellationToken),
                LazyThreadSafetyMode.ExecutionAndPublication));

        try
        {
            return await lazyResult.Value.ConfigureAwait(false);
        }
        catch
        {
            _results.TryRemove(
                new KeyValuePair<ModuleId, Lazy<Task<SerializedModuleResult>>>(moduleId, lazyResult));
            throw;
        }
    }
}

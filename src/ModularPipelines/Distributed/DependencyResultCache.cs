using System.Collections.Concurrent;

namespace ModularPipelines.Distributed;

internal sealed class DependencyResultCache(
    IDistributedWorkerCoordinator coordinator,
    CancellationToken cancellationToken)
{
    private readonly ConcurrentDictionary<string, Lazy<Task<SerializedModuleResult>>> _results =
        new(StringComparer.Ordinal);

    public Task<SerializedModuleResult> GetAsync(string moduleTypeName) =>
        _results.GetOrAdd(
            moduleTypeName,
            name => new Lazy<Task<SerializedModuleResult>>(
                () => coordinator.WaitForResultAsync(name, cancellationToken),
                LazyThreadSafetyMode.ExecutionAndPublication)).Value;
}

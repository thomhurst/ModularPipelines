using System.Collections.Concurrent;

namespace ModularPipelines.Distributed;

internal sealed class DependencyResultCache(
    IDistributedWorkerCoordinator coordinator,
    CancellationToken cancellationToken)
{
    private readonly ConcurrentDictionary<string, Lazy<Task<SerializedModuleResult>>> _results =
        new(StringComparer.Ordinal);

    public async Task<SerializedModuleResult> GetAsync(string moduleTypeName)
    {
        var lazyResult = _results.GetOrAdd(
            moduleTypeName,
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
                new KeyValuePair<string, Lazy<Task<SerializedModuleResult>>>(moduleTypeName, lazyResult));
            throw;
        }
    }
}

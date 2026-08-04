using System.Collections.Concurrent;

namespace ModularPipelines.Engine;

internal sealed class CommandExecutionCounter : ICommandExecutionCounter
{
    private readonly ConcurrentDictionary<Type, int> _counts = new();
    private readonly ConcurrentDictionary<(int WorkerIndex, Type ModuleType), int> _remoteCounts = new();
    private int _totalCount;
    private int _unattributedCount;

    public int TotalCount => Volatile.Read(ref _totalCount);

    public int UnattributedCount => Volatile.Read(ref _unattributedCount);

    public void Record(Type? moduleType) => Add(moduleType, 1);

    public void Add(Type? moduleType, int count)
    {
        if (count <= 0)
        {
            return;
        }

        Interlocked.Add(ref _totalCount, count);
        if (moduleType is null)
        {
            Interlocked.Add(ref _unattributedCount, count);
            return;
        }

        _counts.AddOrUpdate(moduleType, count, (_, currentCount) => currentCount + count);
    }

    public void AddRemote(Type moduleType, int workerIndex, int count)
    {
        if (count <= 0)
        {
            return;
        }

        Add(moduleType, count);
        _remoteCounts.AddOrUpdate(
            (workerIndex, moduleType),
            count,
            (_, currentCount) => currentCount + count);
    }

    public int GetCount(Type moduleType) => _counts.GetValueOrDefault(moduleType);

    public IReadOnlyDictionary<Type, int> GetModuleCounts() =>
        _counts.ToDictionary();

    public IReadOnlyDictionary<(int WorkerIndex, Type ModuleType), int> GetRemoteModuleCounts() =>
        _remoteCounts.ToDictionary();
}

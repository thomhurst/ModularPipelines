using System.Collections.Concurrent;

namespace ModularPipelines.Engine;

internal sealed class CommandExecutionCounter : ICommandExecutionCounter
{
    private readonly ConcurrentDictionary<Type, int> _counts = new();
    private readonly ConcurrentDictionary<(Distributed.WorkerId WorkerId, Type ModuleType), int> _remoteCounts = new();
    private readonly ConcurrentQueue<Reporting.CommandResourceUsage> _resourceUsage = new();
    private readonly ConcurrentDictionary<Type, ConcurrentQueue<Reporting.CommandResourceUsage>> _moduleResourceUsage = new();
    private int _totalCount;
    private int _unattributedCount;

    public void RecordResourceUsage(Type? moduleType, TimeSpan? totalProcessorTime, long? peakWorkingSetBytes)
    {
        var usage = new Reporting.CommandResourceUsage
        {
            ModuleId = moduleType is null ? null : ModuleId.FromType(moduleType),
            TotalProcessorTime = totalProcessorTime,
            PeakWorkingSetBytes = peakWorkingSetBytes,
        };
        _resourceUsage.Enqueue(usage);
        if (moduleType is not null)
        {
            _moduleResourceUsage.GetOrAdd(moduleType, static _ => new()).Enqueue(usage);
        }
    }

    public IReadOnlyList<Reporting.CommandResourceUsage> GetResourceUsage() =>
        _resourceUsage.ToArray();

    public IReadOnlyList<Reporting.CommandResourceUsage> GetResourceUsage(Type moduleType) =>
        _moduleResourceUsage.TryGetValue(moduleType, out var usage) ? usage.ToArray() : [];

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

    public void AddRemote(Type moduleType, Distributed.WorkerId workerId, int count)
    {
        if (count <= 0)
        {
            return;
        }

        Add(moduleType, count);
        _remoteCounts.AddOrUpdate(
            (workerId, moduleType),
            count,
            (_, currentCount) => currentCount + count);
    }

    public int GetCount(Type moduleType) => _counts.GetValueOrDefault(moduleType);

    public IReadOnlyDictionary<Type, int> GetModuleCounts() =>
        _counts.ToDictionary();

    public IReadOnlyDictionary<(Distributed.WorkerId WorkerId, Type ModuleType), int> GetRemoteModuleCounts() =>
        _remoteCounts.ToDictionary();
}

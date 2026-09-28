namespace ModularPipelines.Engine;

internal interface ICommandExecutionCounter
{
    int TotalCount { get; }

    int UnattributedCount { get; }

    void Record(Type? moduleType);

    void Add(Type? moduleType, int count);

    void AddRemote(Type moduleType, Distributed.WorkerId workerId, int count);

    int GetCount(Type moduleType);

    IReadOnlyDictionary<Type, int> GetModuleCounts();

    IReadOnlyDictionary<(Distributed.WorkerId WorkerId, Type ModuleType), int> GetRemoteModuleCounts();
}

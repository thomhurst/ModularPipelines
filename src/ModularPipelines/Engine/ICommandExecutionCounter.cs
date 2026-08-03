namespace ModularPipelines.Engine;

internal interface ICommandExecutionCounter
{
    int TotalCount { get; }

    int UnattributedCount { get; }

    void Record(Type? moduleType);

    void Add(Type? moduleType, int count);

    int GetCount(Type moduleType);

    IReadOnlyDictionary<Type, int> GetModuleCounts();
}

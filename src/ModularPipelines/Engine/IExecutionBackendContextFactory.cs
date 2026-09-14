using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IExecutionBackendContextFactory
{
    IExecutionBackendContext Create(
        IExecutionBackendContext resultContext,
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
        EngineCancellationToken engineCancellationToken);
}

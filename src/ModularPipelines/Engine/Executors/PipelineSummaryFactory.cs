using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Factory for creating <see cref="PipelineSummary"/> instances with all required dependencies.
/// </summary>
internal class PipelineSummaryFactory : IPipelineSummaryFactory
{
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IParallelLimitProvider _parallelLimitProvider;

    public PipelineSummaryFactory(
        IModuleResultRegistry resultRegistry,
        IMetricsCollector metricsCollector,
        IParallelLimitProvider parallelLimitProvider)
    {
        _resultRegistry = resultRegistry;
        _metricsCollector = metricsCollector;
        _parallelLimitProvider = parallelLimitProvider;
    }

    /// <inheritdoc />
    public PipelineSummary Create(
        IReadOnlyList<IModule> allModules,
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        return Create(
            allModules,
            _resultRegistry.GetCompletedResults(allModules),
            totalDuration,
            start,
            end);
    }

    /// <inheritdoc />
    public PipelineSummary Create(
        IReadOnlyList<IModule> modules,
        IReadOnlyList<IModuleResult> results,
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        // Scope metrics to the reported modules: the collector also holds planned modules this
        // backend never claimed.
        var moduleTypes = modules.Select(module => module.GetType()).ToArray();
        return new PipelineSummary(
            modules,
            results,
            totalDuration,
            start,
            end,
            _metricsCollector.ComputeMetrics(start, end, _parallelLimitProvider.GetMaxDegreeOfParallelism(), moduleTypes),
            _metricsCollector.GetTimelines(moduleTypes));
    }
}

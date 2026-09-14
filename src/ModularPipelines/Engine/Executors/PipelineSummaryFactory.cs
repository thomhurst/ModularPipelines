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
        var results = _resultRegistry.GetCompletedResults(allModules);

        return new PipelineSummary(
            allModules,
            results,
            totalDuration,
            start,
            end,
            _metricsCollector.ComputeMetrics(start, end, _parallelLimitProvider.GetMaxDegreeOfParallelism()),
            _metricsCollector.GetTimelines());
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Default implementation of module scheduler factory.
/// </summary>
internal class ModuleSchedulerFactory : IModuleSchedulerFactory
{
    private readonly ILogger<ModuleScheduler> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IOptions<SchedulerOptions> _schedulerOptions;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IModuleConstraintEvaluator _constraintEvaluator;

    public ModuleSchedulerFactory(
        ILogger<ModuleScheduler> logger,
        TimeProvider timeProvider,
        IOptions<SchedulerOptions> schedulerOptions,
        IModuleDependencyRegistry dependencyRegistry,
        IMetricsCollector metricsCollector,
        IModuleConstraintEvaluator constraintEvaluator)
    {
        _logger = logger;
        _timeProvider = timeProvider;
        _schedulerOptions = schedulerOptions;
        _dependencyRegistry = dependencyRegistry;
        _metricsCollector = metricsCollector;
        _constraintEvaluator = constraintEvaluator;
    }

    public IModuleScheduler Create()
    {
        return new ModuleScheduler(_logger, _timeProvider, _schedulerOptions, _dependencyRegistry, _metricsCollector, _constraintEvaluator);
    }
}

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
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IModuleConstraintEvaluator _constraintEvaluator;
    private readonly ILoggerFactory _loggerFactory;

    public ModuleSchedulerFactory(
        ILogger<ModuleScheduler> logger,
        TimeProvider timeProvider,
        IOptions<SchedulerOptions> schedulerOptions,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IMetricsCollector metricsCollector,
        IModuleConstraintEvaluator constraintEvaluator,
        ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _timeProvider = timeProvider;
        _schedulerOptions = schedulerOptions;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _metricsCollector = metricsCollector;
        _constraintEvaluator = constraintEvaluator;
        _loggerFactory = loggerFactory;
    }

    public IModuleScheduler Create()
    {
        var statusReporter = new SchedulerStatusReporter(
            _loggerFactory.CreateLogger<SchedulerStatusReporter>(),
            _timeProvider);

        return new ModuleScheduler(
            _logger,
            _timeProvider,
            _schedulerOptions,
            _dependencyRegistry,
            _metadataRegistry,
            _metricsCollector,
            _constraintEvaluator,
            statusReporter);
    }
}

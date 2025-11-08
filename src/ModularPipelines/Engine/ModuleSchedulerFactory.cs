using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Default implementation of module scheduler factory
/// </summary>
internal class ModuleSchedulerFactory : IModuleSchedulerFactory
{
    private readonly ILogger<ModuleScheduler> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IOptions<SchedulerOptions> _schedulerOptions;

    public ModuleSchedulerFactory(
        ILogger<ModuleScheduler> logger,
        TimeProvider timeProvider,
        IOptions<SchedulerOptions> schedulerOptions)
    {
        _logger = logger;
        _timeProvider = timeProvider;
        _schedulerOptions = schedulerOptions;
    }

    public IModuleScheduler Create()
    {
        return new ModuleScheduler(_logger, _timeProvider, _schedulerOptions);
    }
}

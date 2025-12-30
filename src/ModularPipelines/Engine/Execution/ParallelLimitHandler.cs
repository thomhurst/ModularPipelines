using System.Reflection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for managing parallel execution limits.
/// </summary>
internal class ParallelLimitHandler : IParallelLimitHandler
{
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly ILogger<ParallelLimitHandler> _logger;

    public ParallelLimitHandler(
        IParallelLimitProvider parallelLimitProvider,
        ILogger<ParallelLimitHandler> logger)
    {
        _parallelLimitProvider = parallelLimitProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IDisposable> AcquireParallelLimitAsync(Type moduleType)
    {
        var parallelLimiterAttribute =
            moduleType.GetCustomAttributes<ParallelLimiterAttribute>().FirstOrDefault();

        if (parallelLimiterAttribute != null)
        {
            _logger.LogDebug(
                "Module {ModuleName} acquiring parallel limit from {LimiterType}",
                MarkupFormatter.FormatModuleName(moduleType.Name),
                parallelLimiterAttribute.Type.Name);

            // Use the attribute's GetLock method to avoid reflection on IParallelLimit
            return await parallelLimiterAttribute.GetLock(_parallelLimitProvider).WaitAsync().ConfigureAwait(false);
        }

        return NoOpDisposable.Instance;
    }

    /// <inheritdoc />
    public async Task<IDisposable> AcquireExecutionTypeLimitAsync(ModuleState moduleState)
    {
        var executionTypeLock = _parallelLimitProvider.GetExecutionTypeLock(moduleState.ExecutionType);

        if (executionTypeLock != null)
        {
            _logger.LogDebug(
                "Module {ModuleName} waiting for {ExecutionType} execution slot",
                MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name),
                moduleState.ExecutionType);

            return await executionTypeLock.WaitAsync().ConfigureAwait(false);
        }

        return NoOpDisposable.Instance;
    }
}

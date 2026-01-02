using Microsoft.Extensions.Logging;

namespace ModularPipelines.Engine;

/// <summary>
/// Configures the thread pool for optimal pipeline execution.
/// </summary>
/// <remarks>
/// This service ensures adequate parallelism for module execution by configuring
/// the thread pool's minimum threads. It only increases the minimum if the current
/// value is lower than desired.
/// </remarks>
internal class ThreadPoolConfigurator : IThreadPoolConfigurator
{
    private readonly ILogger<ThreadPoolConfigurator> _logger;

    public ThreadPoolConfigurator(ILogger<ThreadPoolConfigurator> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void Configure()
    {
        // Get current thread pool settings
        ThreadPool.GetMinThreads(out var currentWorkerThreads, out var currentCompletionPortThreads);

        // Calculate desired MinThreads to ensure adequate parallelism
        // Use ProcessorCount * 4 to provide headroom for parallel module execution
        var desiredMinThreads = Environment.ProcessorCount * 4;

        // Only increase if current value is lower
        if (currentWorkerThreads < desiredMinThreads)
        {
            ThreadPool.SetMinThreads(desiredMinThreads, currentCompletionPortThreads);
            _logger.LogDebug(
                "Configured ThreadPool MinThreads from {OldValue} to {NewValue} (ProcessorCount: {ProcessorCount})",
                currentWorkerThreads,
                desiredMinThreads,
                Environment.ProcessorCount);
        }
        else
        {
            _logger.LogDebug(
                "ThreadPool MinThreads already adequate: {CurrentValue} (ProcessorCount: {ProcessorCount})",
                currentWorkerThreads,
                Environment.ProcessorCount);
        }
    }
}

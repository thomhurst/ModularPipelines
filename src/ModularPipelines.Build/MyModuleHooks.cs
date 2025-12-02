using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.Build;

public class MyModuleHooks : IPipelineModuleHooks
{
    // Use ConcurrentDictionary for thread-safe access from parallel module hooks
    private readonly ConcurrentDictionary<string, DateTimeOffset> _moduleStartTimes = new();

    /// <inheritdoc/>
    public Task OnBeforeModuleStartAsync(IPipelineHookContext pipelineContext, IModule module)
    {
        var moduleName = module.GetType().Name;
        var startTime = DateTimeOffset.UtcNow;
        _moduleStartTimes[moduleName] = startTime;
        pipelineContext.Logger.LogInformation("{Module} is starting at {DateTime}", moduleName, startTime);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task OnAfterModuleEndAsync(IPipelineHookContext pipelineContext, IModule module)
    {
        var moduleName = module.GetType().Name;
        var endTime = DateTimeOffset.UtcNow;
        var duration = _moduleStartTimes.TryGetValue(moduleName, out var startTime)
            ? endTime - startTime
            : TimeSpan.Zero;
        pipelineContext.Logger.LogInformation("{Module} finished at {DateTime} after {Elapsed}", moduleName, endTime, duration);
        return Task.CompletedTask;
    }
}
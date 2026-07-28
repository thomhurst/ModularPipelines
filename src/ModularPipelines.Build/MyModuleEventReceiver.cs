using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Build;

public class MyModuleEventReceiver : IModuleEventReceiver
{
    // Use ConcurrentDictionary for thread-safe access from parallel module hooks
    private readonly ConcurrentDictionary<string, DateTimeOffset> _moduleStartTimes = new();

    /// <inheritdoc/>
    public Task OnModuleStartAsync(IModuleHookContext context)
    {
        var moduleName = context.ModuleName;
        var startTime = DateTimeOffset.UtcNow;
        _moduleStartTimes[moduleName] = startTime;
        context.Logger.LogInformation("{Module} is starting at {DateTime}", moduleName, startTime);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task OnModuleEndAsync(IModuleHookContext context)
    {
        var moduleName = context.ModuleName;
        var endTime = DateTimeOffset.UtcNow;
        var duration = _moduleStartTimes.TryGetValue(moduleName, out var startTime)
            ? endTime - startTime
            : TimeSpan.Zero;
        context.Logger.LogInformation("{Module} finished at {DateTime} after {Elapsed}", moduleName, endTime, duration);
        return Task.CompletedTask;
    }
}

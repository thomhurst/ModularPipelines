using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class StatusHandler<T> : BaseHandler<T>, IStatusHandler
{
    public StatusHandler(Module<T> module) : base(module)
    {
    }

    public void LogModuleStatus()
    {
        var moduleName = Module.GetType().Name;
        var message = StatusDisplayProvider.FormatStatusMessage(moduleName, Module.Status);

        var logLevel = Module.Status switch
        {
            Status.NotYetStarted => LogLevel.Warning,
            Status.Processing => LogLevel.Error,
            Status.Successful => LogLevel.Information,
            Status.Failed => LogLevel.Error,
            Status.TimedOut => LogLevel.Error,
            Status.Skipped => LogLevel.Warning,
            Status.Unknown => LogLevel.Error,
            Status.IgnoredFailure => LogLevel.Warning,
            Status.PipelineTerminated => LogLevel.Error,
            Status.UsedHistory => LogLevel.Information,
            Status.Retried => LogLevel.Warning,
            _ => LogLevel.Error
        };

        Context.Logger.Log(logLevel, message);
    }
}
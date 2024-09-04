using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
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

        switch (Module.Status)
        {
            case Status.NotYetStarted:
                Context.Logger.LogWarning("Module {Module} never started", moduleName);
                return;
            case Status.Processing:
                Context.Logger.LogError("Module {Module} didn't finish executing", moduleName);
                return;
            case Status.Successful:
                Context.Logger.LogInformation("Module {Module} completed successfully", moduleName);
                return;
            case Status.Failed:
                Context.Logger.LogError("Module {Module} failed", moduleName);
                return;
            case Status.TimedOut:
                Context.Logger.LogError("Module {Module} timed out", moduleName);
                return;
            case Status.Skipped:
                Context.Logger.LogWarning("Module {Module} skipped", moduleName);
                return;
            case Status.Unknown:
                Context.Logger.LogError("Unknown {Module} module status", moduleName);
                return;
            case Status.IgnoredFailure:
                Context.Logger.LogError("Module {Module} failed but the failure was ignored, so this will not cause the pipeline to error", moduleName);
                return;
            case Status.PipelineTerminated:
                Context.Logger.LogError("The pipeline has errored so Module {Module} will terminate", moduleName);
                return;
            case Status.UsedHistory:
                Context.Logger.LogError("Module {Module} has been constructed from historical data", moduleName);
                return;
            case Status.Retried:
                Context.Logger.LogError("Module {Module} retried", moduleName);
                break;
            default:
                Context.Logger.LogError("Module {Module} status is: {Status}", moduleName, Module.Status);
                return;
        }
    }
}
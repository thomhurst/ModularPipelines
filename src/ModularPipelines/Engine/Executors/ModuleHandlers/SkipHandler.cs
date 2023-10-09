using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class SkipHandler : BaseHandler
{
    internal readonly Task SkippedTask = new(() => { });

    public SkipHandler(ModuleBase module) : base(module)
    {
    }

    public async Task<bool> IsSkipped(SkipDecision skipDecision)
    {
        if (skipDecision.ShouldSkip)
        {
            await SetSkipped(skipDecision);
            return true;
        }

        return false;
    }
    
    internal async Task SetSkipped(SkipDecision skipDecision)
    {
        Module.Status = Status.Skipped;

        Module.SkipResult = skipDecision;

        if (await Module.HistoryHandler.UseResultFromHistoryIfSkipped(Context)
            && await Module.HistoryHandler.SetupModuleFromHistory(skipDecision.Reason))
        {
            return;
        }

        SkippedTask.Start(TaskScheduler.Default);

        ModuleResultTaskCompletionSource.TrySetResult(new SkippedModuleResult<T>(this, skipDecision));

        Context.Logger.LogInformation("{Module} ignored because: {Reason} and no historical results were found", GetType().Name, skipDecision.Reason);
    }
}
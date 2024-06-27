using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class SkipHandler<T> : BaseHandler<T>, ISkipHandler
{
    public Task CallbackTask { get; } = new(() => { });

    public SkipHandler(Module<T> module) : base(module)
    {
    }

    public async Task SetSkipped(SkipDecision skipDecision)
    {
        Module.Status = Status.Skipped;

        Module.SkipResult = skipDecision;

        if (await Module.UseResultFromHistoryIfSkipped(Context)
            && await Module.HistoryHandler.SetupModuleFromHistory(skipDecision.Reason))
        {
            return;
        }

        CallbackTask.Start(TaskScheduler.Default);

        ModuleResultTaskCompletionSource.TrySetResult(new SkippedModuleResult<T>(Module, skipDecision));

        Logger.LogInformation("{Module} ignored because: {Reason} and no historical results were found", GetType().Name, skipDecision.Reason ?? "<No reason provided>");
    }

    public async Task<bool> HandleSkipped()
    {
        var skipDecision = await Module.ShouldSkip(Context);

        if (skipDecision.ShouldSkip)
        {
            await SetSkipped(skipDecision);
            return true;
        }

        return false;
    }
}
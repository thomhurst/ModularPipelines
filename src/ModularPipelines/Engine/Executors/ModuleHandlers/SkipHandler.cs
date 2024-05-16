using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class SkipHandler<T> : BaseHandler<T>, ISkipHandler<T>
    where T : class
{
    public Task CallbackTask { get; } = new(() => { });

    public SkipHandler(Module<T> module) : base(module)
    {
    }

    public async Task<ModuleResult<T>?> SetSkipped(SkipDecision skipDecision)
    {
        Module.Status = Status.Skipped;

        Module.SkipResult = skipDecision;

        if (await Module.UseResultFromHistoryIfSkipped(Context)
            && await Module.HistoryHandler.SetupModuleFromHistory(skipDecision.Reason) 
                is { } historicResult)
        {
            return historicResult;
        }

        CallbackTask.Start(TaskScheduler.Default);
        
        Logger.LogInformation("{Module} ignored because: {Reason} and no historical results were found", GetType().Name, skipDecision.Reason);

        Module.SkipTask.Start();
        return new SkippedModuleResult<T>(Module, skipDecision);
    }

    public async Task<ModuleResult<T>?> HandleSkipped()
    {
        var skipDecision = await Module.ShouldSkip(Context);

        if (skipDecision.ShouldSkip)
        {
            return await SetSkipped(skipDecision);
        }

        return null;
    }
}
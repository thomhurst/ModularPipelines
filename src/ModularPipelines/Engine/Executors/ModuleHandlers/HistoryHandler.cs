using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class HistoryHandler<T> : BaseHandler<T>, IHistoryHandler<T>
{
    public HistoryHandler(Module<T> module) : base(module)
    {
    }
    
    public async Task<bool> SetupModuleFromHistory(string? skipDecisionReason)
    {
        if (Context.ModuleResultRepository.GetType() == typeof(NoOpModuleResultRepository))
        {
            Context.Logger.LogDebug("No results repository configured to pull historical results from");
            return false;
        }

        var result = await Context.ModuleResultRepository.GetResultAsync<T>(Module, Context);

        if (result == null)
        {
            return false;
        }
        
        Context.Logger.LogDebug("Setting up module from history");

        Module.Status = Status.UsedHistory;

        Module.Result = result;

        return true;
    }

    public async Task SaveResult(ModuleResult<T> moduleResult)
    {
        try
        {
            Context.Logger.LogDebug("Saving module result");
            
            Module.Result = moduleResult;
            ModuleResultTaskCompletionSource.TrySetResult(moduleResult);

            await Context.ModuleResultRepository.SaveResultAsync(Module, moduleResult, Context);
        }
        catch (Exception e)
        {
            Context.Logger.LogError(e, "Error saving module result to repository");
        }
    }
}
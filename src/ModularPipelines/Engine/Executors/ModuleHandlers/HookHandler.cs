using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class HookHandler<T> : BaseHandler<T>, IHookHandler
{
    public HookHandler(Module<T> module) : base(module)
    {
    }

    public async Task OnBeforeExecute(IPipelineContext context)
    {
        try
        {
            await Module.OnBeforeExecute(context);
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Error in OnBeforeExecute");
        }
    }

    public async Task OnAfterExecute(IPipelineContext context)
    {
        try
        {
            await Module.OnAfterExecute(context);
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Error in OnAfterExecute");
        }
    }
}
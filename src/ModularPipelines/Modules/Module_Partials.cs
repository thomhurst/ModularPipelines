using ModularPipelines.Context;

namespace ModularPipelines.Modules;

public partial class Module<T>
{
    protected virtual Task OnBeforeExecute(IPipelineContext context)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnAfterExecute(IPipelineContext context)
    {
        return Task.CompletedTask;
    }
}

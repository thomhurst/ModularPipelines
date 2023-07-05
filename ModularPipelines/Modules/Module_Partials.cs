using ModularPipelines.Context;

namespace ModularPipelines.Modules;

public partial class Module<T>
{
    protected virtual Task OnBeforeExecute( IModuleContext context )
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnAfterExecute( IModuleContext context )
    {
        return Task.CompletedTask;
    }
}

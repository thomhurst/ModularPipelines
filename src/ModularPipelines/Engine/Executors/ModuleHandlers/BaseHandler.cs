using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class BaseHandler<T>
{
    public ModuleBase<T> Module { get; }

    public TaskCompletionSource<ModuleResult<T>> ModuleResultTaskCompletionSource => Module.ModuleResultTaskCompletionSource;

    public EngineCancellationToken EngineCancellationToken => Module.Context.EngineCancellationToken;

    public CancellationTokenSource ModuleCancellationTokenSource => Module.Context.EngineCancellationToken;

    public ModuleRunType RunType => Module.ModuleRunType;

    protected BaseHandler(ModuleBase<T> module)
    {
        Module = module;
    }
}
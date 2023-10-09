using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class BaseHandler<T> : BaseHandler
{
    public new Module<T> Module { get; }

    public TaskCompletionSource<ModuleResult<T>> ModuleResultTaskCompletionSource => Module.ModuleResultTaskCompletionSource;

    protected BaseHandler(Module<T> module) : base(module)
    {
        Module = module;
    }
}

internal class BaseHandler
{
    public ModuleBase Module { get; }

    public IPipelineContext Context { get; }
    
    public ILogger Logger => Module.Context.Logger;

    public EngineCancellationToken EngineCancellationToken => Module.Context.EngineCancellationToken;

    public CancellationTokenSource ModuleCancellationTokenSource => Module.Context.EngineCancellationToken;

    public ModuleRunType RunType => Module.ModuleRunType;

    protected BaseHandler(ModuleBase module)
    {
        Module = module;
    }
}
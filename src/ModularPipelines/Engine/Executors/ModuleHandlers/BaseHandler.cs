using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class BaseHandler<T>
{
    public Module<T> Module { get; }

    public TaskCompletionSource<ModuleResult<T>> ModuleResultTaskCompletionSource => Module.ModuleResultTaskCompletionSource;

    public IPipelineContext Context => Module.Context;

    public ILogger Logger => Context.Logger;

    public EngineCancellationToken EngineCancellationToken => Context.EngineCancellationToken;

    public CancellationTokenSource ModuleCancellationTokenSource => Module.ModuleCancellationTokenSource;

    public ModuleRunType RunType => Module.ModuleRunType;

    protected BaseHandler(Module<T> module)
    {
        Module = module;
    }
}
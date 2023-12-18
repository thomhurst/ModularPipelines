using ModularPipelines.Modules;

namespace ModularPipelines.Context;

public interface IPipelineContext : IPipelineHookContext
{
    internal TModule? GetModule<TModule>()
        where TModule : ModuleBase;

    internal ModuleBase? GetModule(Type type);
}
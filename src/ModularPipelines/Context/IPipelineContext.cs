using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to pipeline context including modules and configuration.
/// </summary>
public interface IPipelineContext : IPipelineHookContext
{
    internal TModule? GetModule<TModule>()
        where TModule : ModuleBase;

    internal ModuleBase? GetModule(Type type);
}
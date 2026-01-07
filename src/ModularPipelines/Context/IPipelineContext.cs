using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to pipeline context including modules and configuration.
/// </summary>
/// <remarks>
/// This interface is deprecated. Use <see cref="IModuleContext"/> directly in module implementations.
/// IPipelineContext will be removed in a future major version.
/// </remarks>
[Obsolete("Use IModuleContext directly. IPipelineContext will be removed in v2.0. See migration guide at https://github.com/thomhurst/ModularPipelines/issues/1867")]
public interface IPipelineContext : IPipelineHookContext
{
    internal TModule? GetModule<TModule>()
        where TModule : class, IModule;

    internal IModule? GetModule(Type type);
}

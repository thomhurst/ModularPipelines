using ModularPipelines.Context;

namespace ModularPipelines.Engine;

internal interface IPipelineContextProvider
{
    /// <summary>
    /// Gets the context for a module or pipeline operation owner, using reference identity.
    /// Calls without an owner share the pipeline context.
    /// </summary>
    public IPipelineContext GetModuleContext(object? scopeKey = null);
}
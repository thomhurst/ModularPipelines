using ModularPipelines.Context;

namespace ModularPipelines.Engine;

internal interface IPipelineContextProvider
{
    public Task<IPipelineContext> GetModuleContext();
}
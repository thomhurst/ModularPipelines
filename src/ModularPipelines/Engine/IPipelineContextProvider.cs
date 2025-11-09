using ModularPipelines.Context;

namespace ModularPipelines.Engine;

internal interface IPipelineContextProvider
{
    public IPipelineContext GetModuleContext();
}

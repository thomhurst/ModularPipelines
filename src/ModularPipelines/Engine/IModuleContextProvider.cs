using ModularPipelines.Context;

namespace ModularPipelines.Engine;

internal interface IModuleContextProvider
{
    public IPipelineContext GetModuleContext();
}
using ModularPipelines.Context;

namespace ModularPipelines.Interfaces;

public interface IBuildSystemPipelineFileWriter
{
    Task Write(IPipelineHookContext pipelineHookContext);
}
using ModularPipelines.Context;

namespace ModularPipelines.Extensions;

internal static class PipelineContextExtensions
{
    public static NoModulePipelineContext ToNoModulePipelineContext(this IPipelineContext pipelineContext) => new(pipelineContext);
}
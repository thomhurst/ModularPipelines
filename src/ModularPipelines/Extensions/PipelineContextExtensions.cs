using ModularPipelines.Context;

namespace ModularPipelines.Extensions;

internal static class PipelineContextExtensions
{
    public static IPipelineHookContext ToPipelineHookContext(this IPipelineContext pipelineContext) => pipelineContext;
}
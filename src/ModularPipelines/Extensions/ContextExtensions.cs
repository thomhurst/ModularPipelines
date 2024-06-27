using ModularPipelines.Context;
using ModularPipelines.Logging;

namespace ModularPipelines.Extensions;

public static class ContextExtensions
{
    public static void LogOnPipelineEnd(this IPipelineHookContext pipelineContext, string value)
    {
        var afterPipelineLogger = pipelineContext.Get<IAfterPipelineLogger>()!;
        afterPipelineLogger.LogOnPipelineEnd(value);
    }
}
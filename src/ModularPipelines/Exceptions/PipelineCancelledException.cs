using ModularPipelines.Engine;

namespace ModularPipelines.Exceptions;

public class PipelineCancelledException : PipelineException
{
    internal PipelineCancelledException(EngineCancellationToken engineCancellationToken) : base(GenerateMessage(engineCancellationToken))
    {
    }

    private static string? GenerateMessage(EngineCancellationToken engineCancellationToken)
    {
        if (string.IsNullOrWhiteSpace(engineCancellationToken.Reason))
        {
            return "The pipeline has been terminated.";
        }

        return
            $"The pipeline has been terminated. {Environment.NewLine}Cancellation Reason: {engineCancellationToken.Reason}";
    }
}

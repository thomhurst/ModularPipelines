namespace ModularPipelines.Exceptions;

public class FailedRequirementsException : PipelineException
{
    public FailedRequirementsException(string? message) : base(message)
    {
    }
}
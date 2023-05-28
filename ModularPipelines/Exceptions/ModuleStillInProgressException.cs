namespace ModularPipelines.Exceptions;

public class ModuleStillInProgressException : PipelineException
{
    public ModuleStillInProgressException(string? message) : base(message)
    {
    }
}
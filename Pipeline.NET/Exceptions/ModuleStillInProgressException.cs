namespace Pipeline.NET.Exceptions;

public class ModuleStillInProgressException : PipelineException
{
    public ModuleStillInProgressException(string? message) : base(message)
    {
    }
}
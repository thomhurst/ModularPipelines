namespace ModularPipelines.Exceptions;

public class AlwaysRunPostponedException : PipelineException
{
    public AlwaysRunPostponedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
namespace ModularPipelines.Exceptions;

public class ModuleInitializationFailedException : PipelineException
{
    public ModuleInitializationFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
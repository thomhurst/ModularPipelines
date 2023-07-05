namespace ModularPipelines.Exceptions;

public class ModuleNotRegisteredException : PipelineException
{
    public ModuleNotRegisteredException( string? message, Exception? innerException ) : base( message, innerException )
    {
    }
}

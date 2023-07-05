namespace ModularPipelines.Exceptions;

public class ModuleReferencingSelfException : PipelineException
{
    public ModuleReferencingSelfException( string? message ) : base( message )
    {
    }
}

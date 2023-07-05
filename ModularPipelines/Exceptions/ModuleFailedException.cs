using System.Runtime.Serialization;

namespace ModularPipelines.Exceptions;

public class ModuleFailedException : PipelineException
{
    public ModuleFailedException()
    {
    }

    protected ModuleFailedException( SerializationInfo info, StreamingContext context ) : base( info, context )
    {
    }

    public ModuleFailedException( string? message ) : base( message )
    {
    }

    public ModuleFailedException( string? message, Exception? innerException ) : base( message, innerException )
    {
    }
}

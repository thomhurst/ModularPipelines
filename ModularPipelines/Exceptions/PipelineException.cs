using System.Runtime.Serialization;

namespace ModularPipelines.Exceptions;

public class PipelineException : Exception
{
    public PipelineException()
    {
    }

    protected PipelineException( SerializationInfo info, StreamingContext context ) : base( info, context )
    {
    }

    public PipelineException( string? message ) : base( message )
    {
    }

    public PipelineException( string? message, Exception? innerException ) : base( message, innerException )
    {
    }
}

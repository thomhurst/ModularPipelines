using System.Runtime.Serialization;

namespace ModularPipelines.Exceptions;

public class PipelineException : Exception
{
    public PipelineException()
    {
    }

    public PipelineException(string? message) : base(message)
    {
    }

    public PipelineException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")] // add this attribute to the serialization ctor
#endif
    protected PipelineException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
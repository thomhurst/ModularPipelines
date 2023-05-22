namespace Pipeline.NET.Exceptions;

public class DependencyCollisionException : PipelineException
{
    public DependencyCollisionException(string? message) : base(message)
    {
    }
}
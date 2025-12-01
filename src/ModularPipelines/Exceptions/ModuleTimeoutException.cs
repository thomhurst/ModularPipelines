using ModularPipelines.Helpers;

namespace ModularPipelines.Exceptions;

public class ModuleTimeoutException : PipelineException
{
    internal ModuleTimeoutException(Type moduleType, TimeSpan timeout)
        : base($"{moduleType.Name} has timed out after {timeout.ToDisplayString()}")
    {
    }
}

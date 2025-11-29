using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class ModuleTimeoutException : PipelineException
{
    internal ModuleTimeoutException(ModuleBase moduleBase) : base($"{moduleBase.GetType().Name} has timed out after {GetTimeout(moduleBase)}")
    {
    }

    internal ModuleTimeoutException(Type moduleType, TimeSpan timeout)
        : base($"{moduleType.Name} has timed out after {timeout.ToDisplayString()}")
    {
    }

    private static string GetTimeout(ModuleBase moduleBase)
    {
        return moduleBase.Timeout.ToDisplayString();
    }
}
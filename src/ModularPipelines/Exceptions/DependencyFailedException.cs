using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class DependencyFailedException : PipelineException
{
    public DependencyFailedException(Exception exception, ModuleBase moduleBase) : base($"The dependency {moduleBase.GetType().Name} has failed.", exception)
    {
    }
}
using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class SubModuleFailedException : PipelineException
{
    public SubModuleFailedException(string message) : base(message)
    {
    }

    public SubModuleFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public SubModuleFailedException(SubModuleBase submodule, Exception exception) :
        base($"The Sub-Module {submodule.Name} has failed.", exception)
    {
    }
}
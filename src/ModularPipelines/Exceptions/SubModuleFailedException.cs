using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class SubModuleFailedException : PipelineException
{
    public SubModuleFailedException(SubModuleBase submodule, Exception exception) : 
        base($"The Sub-Module {submodule.Name} has failed.", exception)
    {
    }
}
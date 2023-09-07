using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class ModuleFailedException : PipelineException
{
    public ModuleBase Module { get; }
    
    public ModuleFailedException(ModuleBase module, Exception exception) : base($"The module {module.GetType().Name} has failed.", exception)
    {
        Module = module;
    }
}

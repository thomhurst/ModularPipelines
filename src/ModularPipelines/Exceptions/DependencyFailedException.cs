using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class DependencyFailedException : PipelineException
{
    private readonly ModuleBase _moduleBase;

    public DependencyFailedException(Exception exception, ModuleBase moduleBase) : base($"The dependency {GetInnerMostFailingModule(moduleBase, exception).GetType().Name} has failed.", exception)
    {
        _moduleBase = moduleBase;
    }
    
    private static ModuleBase GetInnerMostFailingModule(ModuleBase rootModuleBase, Exception rootException)
    {
        var module = rootModuleBase;
        
        var exception = rootException;
        
        while (exception != null)
        {
            if (exception is DependencyFailedException dependencyFailedException)
            {
                module = dependencyFailedException._moduleBase;
            }

            exception = exception.InnerException;
        }

        return module;
    }
}
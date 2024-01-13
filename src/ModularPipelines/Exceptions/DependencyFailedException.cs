using System.Text.Json.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class DependencyFailedException : PipelineException
{
    [JsonInclude]
    public string FailingModuleName { get; private set; }

    public DependencyFailedException(Exception exception, ModuleBase moduleBase) : base($"The dependency {GetInnerMostFailingModule(moduleBase, exception)} has failed.", exception)
    {
        FailingModuleName = moduleBase.GetType().Name;
    }
    
    private static string GetInnerMostFailingModule(ModuleBase rootModuleBase, Exception rootException)
    {
        var module = rootModuleBase.GetType().Name;
        
        var exception = rootException;
        
        while (exception != null)
        {
            if (exception is DependencyFailedException dependencyFailedException)
            {
                module = dependencyFailedException.FailingModuleName;
            }

            exception = exception.InnerException;
        }

        return module;
    }
}
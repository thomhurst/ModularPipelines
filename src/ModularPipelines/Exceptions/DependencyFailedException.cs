using System.Text.Json.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class DependencyFailedException : PipelineException
{
    [JsonInclude]
    public string FailingModuleName { get; private set; }

    public DependencyFailedException(Exception exception, IModule module) : base($"The dependency {GetInnerMostFailingModule(module, exception)} has failed.", exception)
    {
        FailingModuleName = module.GetType().Name;
    }

    private static string GetInnerMostFailingModule(IModule rootModule, Exception rootException)
    {
        var module = rootModule.GetType().Name;

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

using System.Text.Json.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class DependencyFailedException : PipelineException
{
    [JsonInclude]
    public string FailingModuleName { get; private set; }

    public DependencyFailedException(Exception exception, IModule IModule) : base($"The dependency {GetInnerMostFailingModule(IModule, exception)} has failed.", exception)
    {
        FailingModuleName = IModule.GetType().Name;
    }

    private static string GetInnerMostFailingModule(IModule rootIModule, Exception rootException)
    {
        var module = rootIModule.GetType().Name;

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

using System.Text.Json.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a module fails because one of its dependencies failed during execution.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when a module cannot execute because a module it depends on
/// (declared via <c>[DependsOn]</c> attribute or programmatic dependencies) has failed.
/// The exception contains information about which dependency caused the failure.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When a module's dependency throws an exception during execution</item>
/// <item>When awaiting a dependency that has already failed</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="FailingModuleName"/> - The name of the module that originally failed</item>
/// <item><see cref="Exception.InnerException"/> - The original exception from the failed dependency</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (DependencyFailedException ex)
/// {
///     Console.WriteLine($"Dependency failed: {ex.FailingModuleName}");
///     Console.WriteLine($"Original error: {ex.InnerException?.Message}");
/// }
/// </code>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="ModuleNotRegisteredException"/>
public class DependencyFailedException : PipelineException
{
    /// <summary>
    /// Gets the name of the module that failed, causing this dependency failure.
    /// </summary>
    [JsonInclude]
    public string FailingModuleName { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyFailedException"/> class.
    /// </summary>
    /// <param name="exception">The exception that caused the dependency to fail.</param>
    /// <param name="module">The module that failed.</param>
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

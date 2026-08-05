using Microsoft.Extensions.Hosting;
using ModularPipelines.Context.Domains.Environment;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

/// <summary>
/// Provides context about the current pipeline environment.
/// </summary>
public interface IEnvironmentContext
{
    /// <summary>
    /// Gets the name of the environment that this Pipeline is running in
    /// e.g. Development or Production.
    /// </summary>
    public string EnvironmentName { get; }

    /// <summary>
    /// Gets the current operating system.
    /// </summary>
    public OperatingSystemIdentifier OperatingSystem { get; }

    /// <summary>
    /// Gets the version of the current operating system.
    /// </summary>
    public Version OperatingSystemVersion { get; }

    /// <summary>
    /// Gets a value indicating whether the current operating system is 64-bit.
    /// </summary>
    public bool Is64BitOperatingSystem { get; }

    /// <inheritdoc cref="System.AppDomain.BaseDirectory"/>
    public Folder AppDomainDirectory { get; }

    /// <inheritdoc cref="IHostEnvironment.ContentRootPath"/>
    /// <remarks>
    /// This property is immutable after pipeline initialization.
    /// </remarks>
    public Folder ContentDirectory { get; }

    /// <summary>
    /// Gets the pipeline's configured working directory.
    /// </summary>
    /// <remarks>
    /// This property is immutable after pipeline initialization. Set
    /// <see cref="PipelineBuilderOptions.WorkingDirectory"/> when creating the pipeline,
    /// or override an individual command with <see cref="Options.CommandExecutionOptions.WorkingDirectory"/>.
    /// </remarks>
    public Folder WorkingDirectory { get; }

    /// <inheritdoc cref="Environment.GetFolderPath(System.Environment.SpecialFolder)"/>
    public Folder? GetFolder(Environment.SpecialFolder specialFolder);

    /// <summary>
    /// Gets the Environment Variables available to this Pipeline.
    /// </summary>
    public IEnvironmentVariablesContext EnvironmentVariables { get; }
}

using Microsoft.Extensions.Hosting;
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
    /// Gets a value indicating whether whether the current operating system is 64 bit.
    /// </summary>
    public bool Is64BitOperatingSystem { get; }

    /// <inheritdoc cref="System.AppDomain.BaseDirectory"/>
    public Folder AppDomainDirectory { get; }

    /// <inheritdoc cref="IHostEnvironment.ContentRootPath"/>
    /// <remarks>
    /// This property is immutable after pipeline initialization.
    /// If you need to change the working directory for command execution,
    /// use command options or <see cref="System.Environment.CurrentDirectory"/> directly.
    /// </remarks>
    public Folder ContentDirectory { get; }

    /// <inheritdoc cref="Environment.CurrentDirectory"/>
    /// <remarks>
    /// This property captures the working directory at pipeline initialization time and is immutable.
    /// If you need to change the working directory for command execution,
    /// use command options or <see cref="System.Environment.CurrentDirectory"/> directly.
    /// </remarks>
    public Folder WorkingDirectory { get; }

    /// <inheritdoc cref="Environment.GetFolderPath(System.Environment.SpecialFolder)"/>
    public Folder? GetFolder(Environment.SpecialFolder specialFolder);

    /// <summary>
    /// Gets the Environment Variables available to this Pipeline.
    /// </summary>
    public IEnvironmentVariables EnvironmentVariables { get; }
}
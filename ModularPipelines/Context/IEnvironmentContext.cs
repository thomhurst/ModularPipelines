using Microsoft.Extensions.Hosting;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

/// <summary>
/// Provides context about the current pipeline environment
/// </summary>
public interface IEnvironmentContext
{
    /// <summary>
    /// The name of the environment that this Pipeline is running in
    /// e.g. Development or Production
    /// </summary>
    public string EnvironmentName { get; }

    /// <summary>
    /// The current operating system
    /// </summary>
    public OperatingSystem OperatingSystem { get; }

    /// <summary>
    /// Whether the current operating system is 64 bit
    /// </summary>
    public bool Is64BitOperatingSystem { get; }

    /// <inheritdoc cref="System.AppDomain.BaseDirectory"/>
    public Folder AppDomainDirectory { get; }

    /// <inheritdoc cref="IHostEnvironment.ContentRootPath"/>
    public Folder ContentDirectory { get; set; }

    /// <inheritdoc cref="Environment.CurrentDirectory"/>
    public Folder WorkingDirectory { get; set; }

    /// <summary>
    /// Gets the Root Folder of the current Git repository
    /// </summary>
    public Folder? GitRootDirectory { get; set; }

    /// <inheritdoc cref="Environment.GetFolderPath(System.Environment.SpecialFolder)"/>
    public Folder? GetFolder(Environment.SpecialFolder specialFolder)
    {
        return Environment.GetFolderPath(specialFolder);
    }

    /// <summary>
    /// Gets the Environment Variables available to this Pipeline
    /// </summary>
    public IEnvironmentVariables EnvironmentVariables { get; }
}

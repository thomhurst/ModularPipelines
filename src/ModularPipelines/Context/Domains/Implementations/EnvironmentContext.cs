using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context.Domains.Environment;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides environment and system information for the pipeline.
/// </summary>
internal class EnvironmentContext : IEnvironmentContext
{
    /// <summary>
    /// Cached operating system platform detected at class load time.
    /// </summary>
    private static readonly OSPlatform OperatingSystemValue = DetectOperatingSystem();

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvironmentContext"/> class.
    /// </summary>
    /// <param name="variables">The environment variables context.</param>
    /// <param name="buildSystem">The build system context.</param>
    /// <param name="workingDirectory">The configured pipeline working directory.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="fileSystemProvider">The file system provider.</param>
    public EnvironmentContext(
        IEnvironmentVariablesContext variables,
        IBuildSystemContext buildSystem,
        PipelineWorkingDirectory workingDirectory,
        IHostEnvironment hostEnvironment,
        IFileSystemProvider fileSystemProvider)
    {
        Variables = variables;
        BuildSystem = buildSystem;
        WorkingDirectory = new Folder(workingDirectory.Path, fileSystemProvider);
        EnvironmentName = hostEnvironment.EnvironmentName;
        AppDomainDirectory = new Folder(AppDomain.CurrentDomain.BaseDirectory, fileSystemProvider);
        ContentDirectory = new Folder(hostEnvironment.ContentRootPath, fileSystemProvider);
    }

    /// <inheritdoc />
    public OSPlatform OperatingSystem => OperatingSystemValue;

    /// <summary>
    /// Detects the operating system platform.
    /// </summary>
    /// <returns>The detected <see cref="OSPlatform"/>.</returns>
    private static OSPlatform DetectOperatingSystem()
    {
        if (System.OperatingSystem.IsWindows())
        {
            return OSPlatform.Windows;
        }

        if (System.OperatingSystem.IsLinux())
        {
            return OSPlatform.Linux;
        }

        if (System.OperatingSystem.IsMacOS())
        {
            return OSPlatform.OSX;
        }

        if (System.OperatingSystem.IsFreeBSD())
        {
            return OSPlatform.FreeBSD;
        }

        // Default fallback
        return OSPlatform.Linux;
    }

    /// <inheritdoc />
    public Architecture Architecture => RuntimeInformation.ProcessArchitecture;

    /// <inheritdoc />
    public string MachineName => System.Environment.MachineName;

    /// <inheritdoc />
    public string UserName => System.Environment.UserName;

    /// <inheritdoc />
    public Folder WorkingDirectory { get; }

    /// <inheritdoc />
    public string EnvironmentName { get; }

    /// <inheritdoc />
    public Folder AppDomainDirectory { get; }

    /// <inheritdoc />
    public Folder ContentDirectory { get; }

    /// <inheritdoc />
    public IEnvironmentVariablesContext Variables { get; }

    /// <inheritdoc />
    public IBuildSystemContext BuildSystem { get; }
}

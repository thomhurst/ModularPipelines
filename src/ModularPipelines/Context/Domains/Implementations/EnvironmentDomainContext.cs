using System.Runtime.InteropServices;
using ModularPipelines.Context.Domains.Environment;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides environment and system information for the pipeline.
/// </summary>
internal class EnvironmentDomainContext : IEnvironmentDomainContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentDomainContext"/> class.
    /// </summary>
    /// <param name="variables">The environment variables context.</param>
    /// <param name="buildSystem">The build system context.</param>
    public EnvironmentDomainContext(
        IEnvironmentVariablesContext variables,
        IBuildSystemContext buildSystem)
    {
        Variables = variables;
        BuildSystem = buildSystem;
        WorkingDirectory = System.Environment.CurrentDirectory;
    }

    /// <inheritdoc />
    public OSPlatform OperatingSystem
    {
        get
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
    }

    /// <inheritdoc />
    public Architecture Architecture => RuntimeInformation.ProcessArchitecture;

    /// <inheritdoc />
    public string MachineName => System.Environment.MachineName;

    /// <inheritdoc />
    public string UserName => System.Environment.UserName;

    /// <inheritdoc />
    public string WorkingDirectory { get; set; }

    /// <inheritdoc />
    public IEnvironmentVariablesContext Variables { get; }

    /// <inheritdoc />
    public IBuildSystemContext BuildSystem { get; }
}

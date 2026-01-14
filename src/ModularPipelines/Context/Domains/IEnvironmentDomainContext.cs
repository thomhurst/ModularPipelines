using System.Runtime.InteropServices;
using ModularPipelines.Context.Domains.Environment;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides environment and system information.
/// </summary>
public interface IEnvironmentDomainContext
{
    /// <summary>
    /// The current operating system.
    /// </summary>
    OSPlatform OperatingSystem { get; }

    /// <summary>
    /// The processor architecture.
    /// </summary>
    Architecture Architecture { get; }

    /// <summary>
    /// The machine name.
    /// </summary>
    string MachineName { get; }

    /// <summary>
    /// The current user name.
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// The current working directory.
    /// </summary>
    string WorkingDirectory { get; set; }

    /// <summary>
    /// Environment variable operations.
    /// </summary>
    IEnvironmentVariablesContext Variables { get; }

    /// <summary>
    /// CI/CD build system detection.
    /// </summary>
    IBuildSystemContext BuildSystem { get; }
}

using System.Runtime.InteropServices;
using ModularPipelines.Context.Domains.Environment;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides environment and system information.
/// </summary>
public interface IEnvironmentDomainContext
{
    /// <summary>
    /// Gets the current operating system.
    /// </summary>
    OSPlatform OperatingSystem { get; }

    /// <summary>
    /// Gets the processor architecture.
    /// </summary>
    Architecture Architecture { get; }

    /// <summary>
    /// Gets the machine name.
    /// </summary>
    string MachineName { get; }

    /// <summary>
    /// Gets the current user name.
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// Gets the working directory captured when the pipeline context was created.
    /// </summary>
    /// <remarks>
    /// To run a command in another directory, set
    /// <see cref="Options.CommandExecutionOptions.WorkingDirectory"/>.
    /// </remarks>
    string WorkingDirectory { get; }

    /// <summary>
    /// Gets environment variable operations.
    /// </summary>
    IEnvironmentVariablesContext Variables { get; }

    /// <summary>
    /// Gets CI/CD build system detection.
    /// </summary>
    IBuildSystemContext BuildSystem { get; }
}

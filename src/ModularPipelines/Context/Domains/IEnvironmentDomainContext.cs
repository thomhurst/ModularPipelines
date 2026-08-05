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
    /// Gets the pipeline's configured working directory.
    /// </summary>
    /// <remarks>
    /// Set <see cref="PipelineBuilderOptions.WorkingDirectory"/> when creating the pipeline,
    /// or override an individual command with
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

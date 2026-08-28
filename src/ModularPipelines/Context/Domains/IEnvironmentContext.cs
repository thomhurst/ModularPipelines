using System.Runtime.InteropServices;
using ModularPipelines.Context.Domains.Environment;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides environment and system information.
/// </summary>
public interface IEnvironmentContext
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
    /// Set <see cref="PipelineBuilderSettings.WorkingDirectory"/> when creating the pipeline,
    /// or override an individual command with
    /// <see cref="Options.CommandExecutionOptions.WorkingDirectory"/>.
    /// </remarks>
    Folder WorkingDirectory { get; }

    /// <summary>
    /// Gets the host environment name, such as Development or Production.
    /// </summary>
    string EnvironmentName { get; }

    /// <inheritdoc cref="System.AppDomain.BaseDirectory" />
    Folder AppDomainDirectory { get; }

    /// <summary>
    /// Gets the host content root directory.
    /// </summary>
    Folder ContentDirectory { get; }

    /// <summary>
    /// Gets environment variable operations.
    /// </summary>
    IEnvironmentVariablesContext Variables { get; }

    /// <summary>
    /// Gets CI/CD build system detection.
    /// </summary>
    IBuildSystemContext BuildSystem { get; }
}

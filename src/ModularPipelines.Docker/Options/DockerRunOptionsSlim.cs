// This file provides a slim version of DockerRunOptions that inherits from the shared base class.
// The original generated file is preserved for backward compatibility.

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

/// <summary>
/// Slim version of docker run options that inherits common container configuration from base class.
/// Use this class for new code to reduce duplication.
/// </summary>
[ExcludeFromCodeCoverage]
[CliSubCommand("run")]
public record DockerRunOptionsSlim : DockerContainerConfigOptions
{
    /// <summary>
    /// Run container in background and print container ID
    /// </summary>
    [CliFlag("--detach", ShortForm = "-d")]
    public bool? Detach { get; set; }

    /// <summary>
    /// Override the key sequence for detaching a container
    /// </summary>
    [CliOption("--detach-keys", Format = OptionFormat.EqualsSeparated)]
    public string? DetachKeys { get; set; }

    /// <summary>
    /// Proxy received signals to the process (default true)
    /// </summary>
    [CliFlag("--sig-proxy")]
    public bool? SigProxy { get; set; }
}

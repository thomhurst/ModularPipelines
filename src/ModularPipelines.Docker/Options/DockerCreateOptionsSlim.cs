// This file provides a slim version of DockerCreateOptions that inherits from the shared base class.
// The original generated file is preserved for backward compatibility.

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

/// <summary>
/// Slim version of docker create options that inherits common container configuration from base class.
/// Use this class for new code to reduce duplication.
/// </summary>
[ExcludeFromCodeCoverage]
[CliSubCommand("create")]
public record DockerCreateOptionsSlim : DockerContainerConfigOptions
{
    // docker create has the same options as docker run minus detach-related options
    // All common options are inherited from DockerContainerConfigOptions
}

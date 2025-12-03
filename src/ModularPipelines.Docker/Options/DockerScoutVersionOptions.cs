using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "version")]
[ExcludeFromCodeCoverage]
public record DockerScoutVersionOptions : DockerOptions
{
}

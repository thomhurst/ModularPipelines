using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("network")]
[ExcludeFromCodeCoverage]
public record DockerNetworkOptions : DockerOptions
{
}

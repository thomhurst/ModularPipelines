using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("network")]
[ExcludeFromCodeCoverage]
public record DockerNetworkOptions : DockerOptions
{
}

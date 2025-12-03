using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "integration")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationOptions : DockerOptions
{
}

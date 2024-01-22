using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "integration")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationOptions : DockerOptions
{
}

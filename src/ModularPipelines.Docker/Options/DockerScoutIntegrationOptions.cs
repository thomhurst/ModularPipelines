using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "integration")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationOptions : DockerOptions
{
}

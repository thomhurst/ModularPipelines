using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "config")]
[ExcludeFromCodeCoverage]
public record DockerScoutConfigOptions : DockerOptions
{
}

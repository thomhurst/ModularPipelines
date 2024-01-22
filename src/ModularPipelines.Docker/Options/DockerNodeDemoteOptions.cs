using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node", "demote")]
[ExcludeFromCodeCoverage]
public record DockerNodeDemoteOptions : DockerOptions
{
}

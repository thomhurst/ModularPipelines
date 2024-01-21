using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "repo")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoOptions : DockerOptions
{
}

using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node", "promote")]
[ExcludeFromCodeCoverage]
public record DockerNodePromoteOptions : DockerOptions
{
}

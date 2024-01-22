using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "key")]
[ExcludeFromCodeCoverage]
public record DockerTrustKeyOptions : DockerOptions
{
}

using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network")]
[ExcludeFromCodeCoverage]
public record DockerNetworkOptions : DockerOptions
{
}

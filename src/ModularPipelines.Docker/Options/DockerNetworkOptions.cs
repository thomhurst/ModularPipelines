using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network")]
[ExcludeFromCodeCoverage]
public record DockerNetworkOptions : DockerOptions
{
}

using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "version")]
[ExcludeFromCodeCoverage]
public record DockerScoutVersionOptions : DockerOptions
{
}

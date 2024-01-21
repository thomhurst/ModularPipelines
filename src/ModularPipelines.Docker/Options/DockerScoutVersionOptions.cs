using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "version")]
[ExcludeFromCodeCoverage]
public record DockerScoutVersionOptions : DockerOptions
{
}

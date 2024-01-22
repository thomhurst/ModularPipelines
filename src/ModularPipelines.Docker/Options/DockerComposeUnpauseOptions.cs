using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "unpause")]
[ExcludeFromCodeCoverage]
public record DockerComposeUnpauseOptions : DockerOptions
{
}

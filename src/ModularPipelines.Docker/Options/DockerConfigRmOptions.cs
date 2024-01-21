using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config", "rm")]
[ExcludeFromCodeCoverage]
public record DockerConfigRmOptions : DockerOptions
{
}

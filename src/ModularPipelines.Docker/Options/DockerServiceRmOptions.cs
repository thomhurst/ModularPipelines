using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service", "rm")]
[ExcludeFromCodeCoverage]
public record DockerServiceRmOptions : DockerOptions
{
}

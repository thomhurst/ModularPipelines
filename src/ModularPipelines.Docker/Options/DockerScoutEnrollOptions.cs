using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "enroll")]
[ExcludeFromCodeCoverage]
public record DockerScoutEnrollOptions : DockerOptions
{
}

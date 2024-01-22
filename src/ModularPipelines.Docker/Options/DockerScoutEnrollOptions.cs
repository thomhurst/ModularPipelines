using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "enroll")]
[ExcludeFromCodeCoverage]
public record DockerScoutEnrollOptions : DockerOptions
{
}

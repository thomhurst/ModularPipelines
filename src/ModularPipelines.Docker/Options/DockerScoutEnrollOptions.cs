using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "enroll")]
[ExcludeFromCodeCoverage]
public record DockerScoutEnrollOptions : DockerOptions
{
}

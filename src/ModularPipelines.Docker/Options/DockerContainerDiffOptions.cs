using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("container", "diff")]
[ExcludeFromCodeCoverage]
public record DockerContainerDiffOptions : DockerOptions
{
}

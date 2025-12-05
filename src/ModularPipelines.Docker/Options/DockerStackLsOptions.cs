using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("stack", "ls")]
[ExcludeFromCodeCoverage]
public record DockerStackLsOptions : DockerOptions
{
    [CliOption("--format")]
    public virtual string? Format { get; set; }
}

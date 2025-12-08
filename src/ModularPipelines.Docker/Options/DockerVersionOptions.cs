using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("version")]
[ExcludeFromCodeCoverage]
public record DockerVersionOptions : DockerOptions
{
    [CliOption("--format")]
    public virtual string? Format { get; set; }
}

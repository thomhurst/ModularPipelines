using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("image", "load")]
[ExcludeFromCodeCoverage]
public record DockerImageLoadOptions : DockerOptions
{
    [CliOption("--input")]
    public virtual string? Input { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}

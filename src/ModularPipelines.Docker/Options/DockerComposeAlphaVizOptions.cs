using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "alpha", "viz")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaVizOptions : DockerOptions
{
    [CliOption("--image")]
    public virtual string? Image { get; set; }

    [CliOption("--indentation-size")]
    public virtual int? IndentationSize { get; set; }

    [CliOption("--networks")]
    public virtual string? Networks { get; set; }

    [CliOption("--ports")]
    public virtual string? Ports { get; set; }

    [CliOption("--spaces")]
    public virtual string? Spaces { get; set; }
}

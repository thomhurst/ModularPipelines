using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha", "viz")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaVizOptions : DockerOptions
{
    [CommandSwitch("--image")]
    public virtual string? Image { get; set; }

    [CommandSwitch("--indentation-size")]
    public virtual int? IndentationSize { get; set; }

    [CommandSwitch("--networks")]
    public virtual string? Networks { get; set; }

    [CommandSwitch("--ports")]
    public virtual string? Ports { get; set; }

    [CommandSwitch("--spaces")]
    public virtual string? Spaces { get; set; }
}

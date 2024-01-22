using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha", "viz")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaVizOptions : DockerOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--indentation-size")]
    public int? IndentationSize { get; set; }

    [CommandSwitch("--networks")]
    public string? Networks { get; set; }

    [CommandSwitch("--ports")]
    public string? Ports { get; set; }

    [CommandSwitch("--spaces")]
    public string? Spaces { get; set; }
}

using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha", "scale")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaScaleOptions : DockerOptions
{
    [BooleanCommandSwitch("--no-deps")]
    public bool? NoDeps { get; set; }
}

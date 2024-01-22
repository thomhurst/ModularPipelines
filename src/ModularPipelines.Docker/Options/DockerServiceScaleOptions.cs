using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service", "scale")]
[ExcludeFromCodeCoverage]
public record DockerServiceScaleOptions : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }
}

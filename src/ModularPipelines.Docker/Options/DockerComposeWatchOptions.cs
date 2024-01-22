using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "watch")]
[ExcludeFromCodeCoverage]
public record DockerComposeWatchOptions : DockerOptions
{
    [CommandSwitch("--no-up")]
    public string? NoUp { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}

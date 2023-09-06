using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container logs")]
[ExcludeFromCodeCoverage]
public record DockerContainerLogsOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [BooleanCommandSwitch("--details")]
    public bool? Details { get; set; }

    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [CommandSwitch("--tail")]
    public string? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public bool? Timestamps { get; set; }

    [CommandSwitch("--until")]
    public string? Until { get; set; }
}

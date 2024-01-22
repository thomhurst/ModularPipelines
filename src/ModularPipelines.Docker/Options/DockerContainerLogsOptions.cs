using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerLogsOptions : DockerOptions
{
    public DockerContainerLogsOptions(
        string container
    )
    {
        CommandParts = ["container", "logs"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

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

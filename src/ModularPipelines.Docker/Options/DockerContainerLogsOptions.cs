using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual bool? Details { get; set; }

    [BooleanCommandSwitch("--follow")]
    public virtual bool? Follow { get; set; }

    [CommandSwitch("--since")]
    public virtual string? Since { get; set; }

    [CommandSwitch("--tail")]
    public virtual string? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public virtual bool? Timestamps { get; set; }

    [CommandSwitch("--until")]
    public virtual string? Until { get; set; }
}

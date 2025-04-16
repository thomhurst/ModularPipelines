using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServiceLogsOptions : DockerOptions
{
    public DockerServiceLogsOptions(
        string serviceOrTask
    )
    {
        CommandParts = ["service", "logs"];

        ServiceOrTask = serviceOrTask;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ServiceOrTask { get; set; }

    [BooleanCommandSwitch("--details")]
    public virtual bool? Details { get; set; }

    [BooleanCommandSwitch("--follow")]
    public virtual bool? Follow { get; set; }

    [BooleanCommandSwitch("--no-resolve")]
    public virtual bool? NoResolve { get; set; }

    [BooleanCommandSwitch("--no-task-ids")]
    public virtual bool? NoTaskIds { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--raw")]
    public virtual bool? Raw { get; set; }

    [CommandSwitch("--since")]
    public virtual string? Since { get; set; }

    [CommandSwitch("--tail")]
    public virtual string? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public virtual bool? Timestamps { get; set; }
}

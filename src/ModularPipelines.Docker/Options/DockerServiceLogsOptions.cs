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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? ServiceOrTask { get; set; }

    [CliFlag("--details")]
    public virtual bool? Details { get; set; }

    [CliFlag("--follow")]
    public virtual bool? Follow { get; set; }

    [CliFlag("--no-resolve")]
    public virtual bool? NoResolve { get; set; }

    [CliFlag("--no-task-ids")]
    public virtual bool? NoTaskIds { get; set; }

    [CliFlag("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [CliFlag("--raw")]
    public virtual bool? Raw { get; set; }

    [CliOption("--since")]
    public virtual string? Since { get; set; }

    [CliOption("--tail")]
    public virtual string? Tail { get; set; }

    [CliFlag("--timestamps")]
    public virtual bool? Timestamps { get; set; }
}

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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Container { get; set; }

    [CliFlag("--details")]
    public virtual bool? Details { get; set; }

    [CliFlag("--follow")]
    public virtual bool? Follow { get; set; }

    [CliOption("--since")]
    public virtual string? Since { get; set; }

    [CliOption("--tail")]
    public virtual string? Tail { get; set; }

    [CliFlag("--timestamps")]
    public virtual bool? Timestamps { get; set; }

    [CliOption("--until")]
    public virtual string? Until { get; set; }
}

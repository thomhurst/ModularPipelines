using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerCommitOptions : DockerOptions
{
    public DockerContainerCommitOptions(
        string container
    )
    {
        CommandParts = ["container", "commit"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Container { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Repository { get; set; }

    [CliOption("--author")]
    public virtual string? Author { get; set; }

    [CliOption("--change")]
    public virtual string? Change { get; set; }

    [CliOption("--message")]
    public virtual string? Message { get; set; }

    [CliFlag("--pause")]
    public virtual bool? Pause { get; set; }
}

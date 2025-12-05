using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServiceRollbackOptions : DockerOptions
{
    public DockerServiceRollbackOptions(
        string service
    )
    {
        CommandParts = ["service", "rollback"];

        Service = service;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Service { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}

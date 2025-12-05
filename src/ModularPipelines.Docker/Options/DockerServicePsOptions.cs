using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServicePsOptions : DockerOptions
{
    public DockerServicePsOptions(
        IEnumerable<string> service
    )
    {
        CommandParts = ["service", "ps"];

        Service = service;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--no-resolve")]
    public virtual bool? NoResolve { get; set; }

    [CliFlag("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}

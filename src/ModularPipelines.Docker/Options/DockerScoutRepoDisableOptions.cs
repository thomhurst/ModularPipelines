using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "repo", "disable")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoDisableOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Repository { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--integration")]
    public virtual string? Integration { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--registry")]
    public virtual string? Registry { get; set; }
}

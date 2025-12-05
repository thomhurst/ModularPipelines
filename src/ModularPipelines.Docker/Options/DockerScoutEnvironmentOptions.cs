using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "environment")]
[ExcludeFromCodeCoverage]
public record DockerScoutEnvironmentOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Environment { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Image { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }
}

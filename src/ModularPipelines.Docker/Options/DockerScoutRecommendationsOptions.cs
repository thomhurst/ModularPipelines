using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "recommendations")]
[ExcludeFromCodeCoverage]
public record DockerScoutRecommendationsOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? ImageOrDirectoryOrArchive { get; set; }

    [CliOption("--only-refresh")]
    public virtual string? OnlyRefresh { get; set; }

    [CliOption("--only-update")]
    public virtual string? OnlyUpdate { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--ref")]
    public virtual string? Ref { get; set; }

    [CliOption("--tag")]
    public virtual string? Tag { get; set; }
}

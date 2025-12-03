using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "watch")]
[ExcludeFromCodeCoverage]
public record DockerScoutWatchOptions : DockerOptions
{
    [CliOption("--all-images")]
    public virtual string? AllImages { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliOption("--interval")]
    public virtual int? Interval { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--refresh-registry")]
    public virtual string? RefreshRegistry { get; set; }

    [CliOption("--registry")]
    public virtual string? Registry { get; set; }

    [CliOption("--repository")]
    public virtual string? Repository { get; set; }

    [CliFlag("--sbom")]
    public virtual bool? Sbom { get; set; }

    [CliOption("--tag")]
    public virtual string? Tag { get; set; }

    [CliOption("--workers")]
    public virtual int? Workers { get; set; }
}

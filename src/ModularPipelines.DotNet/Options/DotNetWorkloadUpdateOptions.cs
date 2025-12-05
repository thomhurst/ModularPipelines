using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliSubCommand("workload", "update")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadUpdateOptions : DotNetOptions
{
    [CliFlag("--advertising-manifests-only")]
    public virtual bool? AdvertisingManifestsOnly { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }

    [CliFlag("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [CliFlag("--from-previous-sdk")]
    public virtual bool? FromPreviousSdk { get; set; }

    [CliFlag("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [CliFlag("--include-previews")]
    public virtual bool? IncludePreviews { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliOption("--source")]
    public virtual IEnumerable<string>? Source { get; set; }

    [CliOption("--temp-dir")]
    public virtual string? TempDir { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}

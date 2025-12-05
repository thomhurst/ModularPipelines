using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetWorkloadInstallOptions : DotNetOptions
{
    public DotNetWorkloadInstallOptions(
        string workloadId
    )
    {
        CommandParts = ["workload", "install", "<WORKLOAD_ID>", "..."];

        WorkloadId = workloadId;
    }

    [CliArgument(Name = "<WORKLOAD_ID>")]
    public virtual string? WorkloadId { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }

    [CliFlag("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [CliFlag("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [CliFlag("--include-previews")]
    public virtual bool? IncludePreviews { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliFlag("--skip-manifest-update")]
    public virtual bool? SkipManifestUpdate { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--temp-dir")]
    public virtual string? TempDir { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}

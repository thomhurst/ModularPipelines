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

    [PositionalArgument(PlaceholderName = "<WORKLOAD_ID>")]
    public string? WorkloadId { get; set; }

    [CommandSwitch("--configfile")]
    public virtual string? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--include-previews")]
    public virtual bool? IncludePreviews { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [BooleanCommandSwitch("--skip-manifest-update")]
    public virtual bool? SkipManifestUpdate { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--temp-dir")]
    public virtual string? TempDir { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}

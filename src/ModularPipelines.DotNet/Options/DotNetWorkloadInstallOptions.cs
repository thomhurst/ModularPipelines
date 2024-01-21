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
    public string? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public bool? DisableParallel { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--include-previews")]
    public bool? IncludePreviews { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [BooleanCommandSwitch("--skip-manifest-update")]
    public bool? SkipManifestUpdate { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--temp-dir")]
    public string? TempDir { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}

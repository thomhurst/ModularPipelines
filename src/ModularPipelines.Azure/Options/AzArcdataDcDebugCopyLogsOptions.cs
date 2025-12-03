using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "dc", "debug", "copy-logs")]
public record AzArcdataDcDebugCopyLogsOptions(
[property: CliOption("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CliOption("--container")]
    public string? Container { get; set; }

    [CliFlag("--exclude-arcdata-logs")]
    public bool? ExcludeArcdataLogs { get; set; }

    [CliFlag("--exclude-cluster-info")]
    public bool? ExcludeClusterInfo { get; set; }

    [CliFlag("--exclude-controldb")]
    public bool? ExcludeControldb { get; set; }

    [CliFlag("--exclude-dumps")]
    public bool? ExcludeDumps { get; set; }

    [CliFlag("--exclude-system-logs")]
    public bool? ExcludeSystemLogs { get; set; }

    [CliOption("--pod")]
    public string? Pod { get; set; }

    [CliOption("--resource-kind")]
    public string? ResourceKind { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliFlag("--skip-compress")]
    public bool? SkipCompress { get; set; }

    [CliOption("--target-folder")]
    public string? TargetFolder { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
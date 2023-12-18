using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "debug", "copy-logs")]
public record AzArcdataDcDebugCopyLogsOptions(
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--exclude-arcdata-logs")]
    public bool? ExcludeArcdataLogs { get; set; }

    [BooleanCommandSwitch("--exclude-cluster-info")]
    public bool? ExcludeClusterInfo { get; set; }

    [BooleanCommandSwitch("--exclude-controldb")]
    public bool? ExcludeControldb { get; set; }

    [BooleanCommandSwitch("--exclude-dumps")]
    public bool? ExcludeDumps { get; set; }

    [BooleanCommandSwitch("--exclude-system-logs")]
    public bool? ExcludeSystemLogs { get; set; }

    [CommandSwitch("--pod")]
    public string? Pod { get; set; }

    [CommandSwitch("--resource-kind")]
    public string? ResourceKind { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [BooleanCommandSwitch("--skip-compress")]
    public bool? SkipCompress { get; set; }

    [CommandSwitch("--target-folder")]
    public string? TargetFolder { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}
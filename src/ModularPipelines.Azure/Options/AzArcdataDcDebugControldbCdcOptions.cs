using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "debug", "controldb-cdc")]
public record AzArcdataDcDebugControldbCdcOptions(
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [BooleanCommandSwitch("--enable")]
    public bool? Enable { get; set; }

    [CommandSwitch("--retention-hours")]
    public string? RetentionHours { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}


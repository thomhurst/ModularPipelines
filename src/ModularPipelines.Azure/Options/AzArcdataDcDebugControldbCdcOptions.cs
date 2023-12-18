using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}
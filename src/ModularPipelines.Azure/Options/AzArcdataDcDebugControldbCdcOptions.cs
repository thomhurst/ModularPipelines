using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "debug", "controldb-cdc")]
public record AzArcdataDcDebugControldbCdcOptions(
[property: CliOption("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliOption("--retention-hours")]
    public string? RetentionHours { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "debug", "dump")]
public record AzArcdataDcDebugDumpOptions(
[property: CliOption("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--target-folder")]
    public string? TargetFolder { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
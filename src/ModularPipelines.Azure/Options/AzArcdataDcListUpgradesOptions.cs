using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "list-upgrades")]
public record AzArcdataDcListUpgradesOptions(
[property: CliOption("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
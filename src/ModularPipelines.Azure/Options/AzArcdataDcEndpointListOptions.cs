using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "endpoint", "list")]
public record AzArcdataDcEndpointListOptions(
[property: CliOption("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
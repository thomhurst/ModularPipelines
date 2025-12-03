using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "ad-connector", "show")]
public record AzArcdataAdConnectorShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--data-controller-name")]
    public string? DataControllerName { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
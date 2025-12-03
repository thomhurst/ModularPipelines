using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "ad-connector", "delete")]
public record AzArcdataAdConnectorDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--data-controller-name")]
    public string? DataControllerName { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
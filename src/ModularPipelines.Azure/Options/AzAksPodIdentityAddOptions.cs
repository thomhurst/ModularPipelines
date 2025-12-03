using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "pod-identity", "add")]
public record AzAksPodIdentityAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--identity-resource-id")] string IdentityResourceId,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliOption("--binding-selector")]
    public string? BindingSelector { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}
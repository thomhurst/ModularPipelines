using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "pod-identity", "add")]
public record AzAksPodIdentityAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--identity-resource-id")] string IdentityResourceId,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CommandSwitch("--binding-selector")]
    public string? BindingSelector { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}
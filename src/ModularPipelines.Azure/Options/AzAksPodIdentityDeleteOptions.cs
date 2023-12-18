using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "pod-identity", "delete")]
public record AzAksPodIdentityDeleteOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "pod-identity", "exception", "add")]
public record AzAksPodIdentityExceptionAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--pod-labels")] string PodLabels,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}
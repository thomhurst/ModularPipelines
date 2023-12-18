using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "attach", "kubernetes")]
public record AzMlComputetargetAttachKubernetesOptions(
[property: CommandSwitch("--compute-resource-id")] string ComputeResourceId,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "attach", "aks")]
public record AzMlComputetargetAttachAksOptions(
[property: CommandSwitch("--compute-resource-id")] string ComputeResourceId,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}
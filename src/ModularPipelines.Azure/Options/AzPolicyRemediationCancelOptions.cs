using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "remediation", "cancel")]
public record AzPolicyRemediationCancelOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}
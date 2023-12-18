using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "remediation", "create")]
public record AzPolicyRemediationCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-assignment")] string PolicyAssignment
) : AzOptions
{
    [CommandSwitch("--definition-reference-id")]
    public string? DefinitionReferenceId { get; set; }

    [CommandSwitch("--location-filters")]
    public string? LocationFilters { get; set; }

    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-discovery-mode")]
    public string? ResourceDiscoveryMode { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}
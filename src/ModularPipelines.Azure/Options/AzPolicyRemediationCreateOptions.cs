using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("policy", "remediation", "create")]
public record AzPolicyRemediationCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-assignment")] string PolicyAssignment
) : AzOptions
{
    [CliOption("--definition-reference-id")]
    public string? DefinitionReferenceId { get; set; }

    [CliOption("--location-filters")]
    public string? LocationFilters { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-discovery-mode")]
    public string? ResourceDiscoveryMode { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}
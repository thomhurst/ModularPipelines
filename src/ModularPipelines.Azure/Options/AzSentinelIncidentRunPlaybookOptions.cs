using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "incident", "run-playbook")]
public record AzSentinelIncidentRunPlaybookOptions(
[property: CliOption("--incident-identifier")] string IncidentIdentifier,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--logic-apps-resource-id")]
    public string? LogicAppsResourceId { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }
}
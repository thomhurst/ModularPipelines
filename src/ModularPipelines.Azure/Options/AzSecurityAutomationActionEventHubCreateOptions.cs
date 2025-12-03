using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "automation-action-event-hub", "create")]
public record AzSecurityAutomationActionEventHubCreateOptions(
[property: CliOption("--connection-string")] string ConnectionString,
[property: CliOption("--event-hub-resource-id")] string EventHubResourceId
) : AzOptions
{
    [CliOption("--sas-policy-name")]
    public string? SasPolicyName { get; set; }
}
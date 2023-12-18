using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-action-event-hub", "create")]
public record AzSecurityAutomationActionEventHubCreateOptions(
[property: CommandSwitch("--connection-string")] string ConnectionString,
[property: CommandSwitch("--event-hub-resource-id")] string EventHubResourceId
) : AzOptions
{
    [CommandSwitch("--sas-policy-name")]
    public string? SasPolicyName { get; set; }
}
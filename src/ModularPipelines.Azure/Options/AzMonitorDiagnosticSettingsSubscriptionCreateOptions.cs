using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "diagnostic-settings", "subscription", "create")]
public record AzMonitorDiagnosticSettingsSubscriptionCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--event-hub-auth-rule")]
    public string? EventHubAuthRule { get; set; }

    [CommandSwitch("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logs")]
    public string? Logs { get; set; }

    [CommandSwitch("--service-bus-rule")]
    public string? ServiceBusRule { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}
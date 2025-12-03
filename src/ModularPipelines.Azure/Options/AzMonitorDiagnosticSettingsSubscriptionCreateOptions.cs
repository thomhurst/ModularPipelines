using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "diagnostic-settings", "subscription", "create")]
public record AzMonitorDiagnosticSettingsSubscriptionCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--event-hub-auth-rule")]
    public string? EventHubAuthRule { get; set; }

    [CliOption("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logs")]
    public string? Logs { get; set; }

    [CliOption("--service-bus-rule")]
    public string? ServiceBusRule { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}